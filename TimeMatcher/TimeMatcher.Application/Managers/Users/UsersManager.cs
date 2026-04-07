using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeMatcher.Application.Errors;
using TimeMatcher.Application.Managers.Meetings;
using TimeMatcher.Application.Models.Requests.User;
using TimeMatcher.Application.Models.Responses;
using TimeMatcher.Application.Models.Responses.Group;
using TimeMatcher.Application.Models.Responses.Meeting;
using TimeMatcher.Application.Models.Responses.User;
using TimeMatcher.Domain.AbilityAggregate;
using TimeMatcher.Domain.Enums;
using TimeMatcher.Domain.GroupAggregate;
using TimeMatcher.Domain.MeetingAggregate;
using TimeMatcher.Domain.UserAggregate;
using UserManagerNotOur = Microsoft.AspNetCore.Identity.UserManager<TimeMatcher.Domain.UserAggregate.User>;

namespace TimeMatcher.Application.Managers.Users;

internal class UsersManager(
    UserManager<User> userManager, 
    IAccessTokenGenerator accessTokenGenerator, 
    SignInManager<User> signInManager,
    IUsersRepository userRepository,
    IGroupsRepository groupsRepository,
    IMeetingsRepository meetingsRepository,
    IAbilitiesRepository abilitiesRepository): IUsersManager
{
    public async Task<Result<UserResponse[]>> GetUsers(GetUsersRequest request)
    {
        if(request.Limit<=0 || request.Page<0)
            return Result.Fail(AppError.UnprocessableContent());

        var users = await userRepository.GetAll()
            .Where(user => 
                (request.Email == null && request.UserName == null) || 
                (user.UserName == request.UserName || user.Email == request.Email))
            .Select(user => new UserResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            })
            .Skip(request.Limit * request.Page)
            .Take(request.Limit)
            .ToArrayAsync();
        return Result.Ok(users);
    }

    public async Task<Result<UserResponse>> GetUserById(Guid id, Guid requestUserId)
    {
        var user = await userRepository.Get(id);
        if (user is null)
            return Result.Fail(AppError.NotFound());
        return Result.Ok(new UserResponse
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
        });
    }

    public async Task<Result<GroupResponse[]>> GetUserGroups(Guid id, Guid requestUserId)
    {
        if (id != requestUserId)
            return Result.Fail(AppError.Forbidden());

        var user = await userRepository.Get(id);
        if (user is null)
            return Result.Fail(AppError.NotFound());

        return Result.Ok(await groupsRepository.GetAll()
            .Where(group => group.GroupParticipants.Any(participant => participant.Id == id))
            .Select(group => new GroupResponse
            {
                Id = group.Id,
                Name = group.Name,
                Participants = group.GroupParticipants.Select(gp =>
                    new GroupParticipantResponse
                    {
                        UserId = gp.UserId,
                        UserName = userRepository.GetAll()
                            .Where(u => u.Id == gp.UserId)
                            .FirstOrDefault().UserName,
                        Email = userRepository.GetAll()
                            .Where(u => u.Id == gp.UserId)
                            .FirstOrDefault().Email
                    }
                ).ToArray()
            })
            .ToArrayAsync());
}

    public async Task<Result<MeetingResponse[]>> GetUserMeetings(Guid id, Guid requestUserId)
    {
        if (id != requestUserId)
            return Result.Fail(AppError.Forbidden());

        var user = await userRepository.Get(id);
        if (user is null)
            return Result.Fail(AppError.NotFound());

        return Result.Ok(await meetingsRepository.GetAll()
            .Where(meeting => meeting.MeetingParticipants.Any(participant => participant.Id == id))
            .Select(meeting => new MeetingResponse
            {
                Id = meeting.Id,
                Name = meeting.Name,
                StartTime = meeting.StartTime,
                EndTime = meeting.EndTime,
                Link = meeting.Link,
                Comment = meeting.Comment,
                Participants = meeting.MeetingParticipants.Select(mt =>
                    new MeetingParticipantResponse
                    {
                        UserId = mt.UserId,
                        UserName = userRepository.GetAll()
                            .Where(u => u.Id == mt.UserId)
                            .FirstOrDefault().UserName,
                        Email = userRepository.GetAll()
                            .Where(u => u.Id == mt.UserId)
                            .FirstOrDefault().Email
                    }
                ).ToArray()
            })
            .ToArrayAsync());
    }

    public async Task<Result<CalendarResponse>> GetUserCalendar(Guid id, RequestedPeriod period, Guid requestUserId)
    {
        if (id != requestUserId)
            return Result.Fail(AppError.Forbidden());

        if(period.End<period.Start)
            return Result.Fail(AppError.UnprocessableContent());

        var user = await userRepository.Get(id);
        if (user is null)
            return Result.Fail(AppError.NotFound());

        return Result.Ok(new CalendarResponse
        {
            RequestedPeriod = period,
            Slots = user.Calendar.Slots
                .Where(slot => slot.EndTime >= period.Start && slot.StartTime <= period.End)
                .Select(slot => new SlotResponse
                {
                    Id = slot.Id,
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime,
                    Comment = slot.Comment,
                    Ability = new AbilityResponse
                    {
                        Id = slot.Ability.Id, 
                        Ability = slot.Ability.Name
                    },
                    MeetingId = slot.Meeting?.Id

                })
                .ToArray()
        });
    }

    public async Task<Result<CalendarResponse>> GetMergedCalendar(GetMergedCalendarRequest request, Guid requestUserId)
    {
        if(!request.UserIds.Any(id => id == requestUserId))
            return Result.Fail(AppError.Forbidden());

        if(request.RequestedPeriod.End<request.RequestedPeriod.Start)
            return Result.Fail(AppError.UnprocessableContent());

        var slots = new List<SlotResponse>();

        foreach(var id in request.UserIds)
        {
            var user = await userRepository.Get(id);
            if (user is null)
                return Result.Fail(AppError.NotFound());
            slots.AddRange(user.Calendar.Slots
                .Where(slot => slot.EndTime >= request.RequestedPeriod.Start && slot.StartTime <= request.RequestedPeriod.End)
                .Select(slot => new SlotResponse
                {
                    Id = slot.Id,
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime,
                    Comment = slot.Comment,
                    Ability = new AbilityResponse
                    {
                        Id = slot.Ability.Id, 
                        Ability = slot.Ability.Name
                    },
                    MeetingId = slot.Meeting?.Id

                })
            );
        }

        return Result.Ok(new CalendarResponse
        {
            RequestedPeriod = request.RequestedPeriod,
            Slots = slots.ToArray()
        });
    }

    public async Task<Result<SlotResponse>> CreateSlot(SlotRequest request, Guid userId, Guid requestUserId)
    {
        if (userId != requestUserId)
            return Result.Fail(AppError.Forbidden());

        if(request.EndTime<= request.StartTime)
            return Result.Fail(AppError.UnprocessableContent());

        var user = await userRepository.Get(userId);
        if (user is null)
            return Result.Fail(AppError.NotFound("пользователь не найден"));

        var ability = abilitiesRepository.GetAll().Where(ability => ability.Id == request.AbilityId).FirstOrDefault();
        if (ability is null)
            return Result.Fail(AppError.NotFound("доступность не найдена"));


        var slot = user.Calendar.AddSlot(request.StartTime,request.EndTime,request.Comment,ability,null);

        return Result.Ok(new SlotResponse
        {
            Id = slot.Id,
            StartTime = slot.StartTime,
            EndTime = slot.EndTime,
            Comment = slot.Comment,
            Ability = new AbilityResponse
            {
                Id = slot.Ability.Id, 
                Ability = slot.Ability.Name
            },
            MeetingId = slot.Meeting?.Id
        });
    }

    public async Task<Result<SlotResponse>> UpdateSlot(Guid id, Guid userId, SlotRequest request, Guid requestUserId)
    {
        if (userId != requestUserId)
            return Result.Fail(AppError.Forbidden());

        if(request.EndTime<= request.StartTime)
            return Result.Fail(AppError.UnprocessableContent());

        var user = await userRepository.Get(userId);
        if (user is null)
            return Result.Fail(AppError.NotFound("пользователь не найден"));

        var ability = abilitiesRepository.GetAll().Where(ability => ability.Id == request.AbilityId).FirstOrDefault();
        if (ability is null)
            return Result.Fail(AppError.NotFound("доступность не найдена"));

        var slot = user.Calendar.Slots.FirstOrDefault(slot=> slot.Id == id);
        if (slot is null)
            return Result.Fail(AppError.NotFound("слот не найден"));

        slot.StartTime = request.StartTime;
        slot.EndTime = request.EndTime;
        slot.Comment = request.Comment;
        slot.Ability = ability;

        return Result.Ok(new SlotResponse
        {
            Id = slot.Id,
            StartTime = slot.StartTime,
            EndTime = slot.EndTime,
            Comment = slot.Comment,
            Ability = new AbilityResponse
            {
                Id = slot.Ability.Id, 
                Ability = slot.Ability.Name
            },
            MeetingId = slot.Meeting?.Id
        });
    }

    public async Task<Result<UserResponse>> UpdateUser(Guid id, UpdateUserRequest request, Guid requestUserId)
    {
        if (requestUserId != id)
            return Result.Fail(AppError.Forbidden("Нельзя редактировать не себя"));
        var user = await userManager.FindByIdAsync(id.ToString());
        user.UserName = request.UserName;
        user.Email = request.Email;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return Result.Fail(AppError.UnprocessableContent(
                string.Join(Environment.NewLine, result.Errors.Select(error => error.Description))));
        }

        return Result.Ok(new UserResponse
        {
            Id = user.Id,
            UserName = user.UserName, 
            Email = user.Email
        });
    }

    public async Task<Result> DeleteUser(Guid id, Guid requestUserId)
    {
        if (id != requestUserId)
            return Result.Fail(AppError.Forbidden("Нельзя удалить не себя"));
        var user = await userManager.FindByIdAsync(id.ToString());
        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
        {
            return Result.Fail(AppError.UnprocessableContent(
                string.Join(Environment.NewLine, result.Errors.Select(error => error.Description))));
        }
        return Result.Ok();
    }

    public async Task<Result> DeleteSlot(Guid id, Guid userId, Guid requestUserId)
    {
        if (userId != requestUserId)
            return Result.Fail(AppError.Forbidden());

        var user = await userRepository.Get(userId);
        if (user is null)
            return Result.Fail(AppError.NotFound("пользователь не найден"));

        if(!user.Calendar.Slots.Any(slot => slot.Id == id))
            return Result.Fail(AppError.NotFound("слот не найден"));

        user.Calendar.RemoveSlot(id);
        
        return Result.Ok();
    }
    
    public async Task<Result<LoginInfoResponse>> RegisterAsync(RegisterUserRequest request)
    {
        var user = new User { Email = request.Email, UserName = request.UserName };
        
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Result.Fail(AppError.UnprocessableContent(
                string.Join(Environment.NewLine, result.Errors.Select(error => error.Description))));
        }
        
        await userManager.AddToRoleAsync(user, $"{ServiceRole.User}");
        
        return Result.Ok(new LoginInfoResponse 
        {
            UserId = user.Id,
            AccessToken = await GenerateAccessToken(user)
        });
    }
    
    private async Task<string> GenerateAccessToken(User user)
    {
        var principal = await signInManager.CreateUserPrincipalAsync(user);
        return accessTokenGenerator.Generate(principal.Claims);
    }

    public async Task<Result<LoginInfoResponse>> LoginAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null || !await userManager.CheckPasswordAsync(user, password))
            return Result.Fail(AppError.Unauthorized("Неверный email или password"));
        
        var accessToken = await GenerateAccessToken(user);

        return Result.Ok(new LoginInfoResponse
        {
            UserId = user.Id,
            AccessToken = accessToken,
        });
    }

    public async Task<Result<LoginInfoResponse>> ChangePassword(Guid userId, ChangePasswordRequest request, Guid requestUserId)
    {
        if (userId != requestUserId)
            return Result.Fail(AppError.Forbidden());
        var user = await userManager.FindByIdAsync(requestUserId.ToString());
        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            return Result.Fail(AppError.UnprocessableContent(
                string.Join(Environment.NewLine, result.Errors.Select(error => error.Description))));
        }
        
        var accessToken = await GenerateAccessToken(user);
        return Result.Ok(new LoginInfoResponse
        {
            UserId = user.Id,
            AccessToken = accessToken
        });
    }
}