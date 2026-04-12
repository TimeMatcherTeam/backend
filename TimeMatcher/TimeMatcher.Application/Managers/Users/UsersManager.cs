using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TimeMatcher.Application.Errors;
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


namespace TimeMatcher.Application.Managers.Users;

internal class UsersManager(
    UserManager<User> userManager, 
    IAccessTokenGenerator accessTokenGenerator, 
    SignInManager<User> signInManager,
    IUsersRepository userRepository,
    IGroupsRepository groupsRepository,
    IMeetingsRepository meetingsRepository,
    IAbilitiesRepository abilitiesRepository,
    ISlotsRepository slotsRepository): IUsersManager
{
    public async Task<Result<UserResponse[]>> GetUsers(GetUsersRequest request)
    {
        if(request.Limit<=0 || request.Page<0)
            return Result.Fail(AppError.UnprocessableContent());

        var users = await userRepository.GetAll()
            .Where(user => 
                (request.Email == null && request.UserName == null) || 
                user.UserName == request.UserName || user.Email == request.Email)
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

        var groups = await groupsRepository.GetAll()
            .Where(group => group.GroupParticipants.Any(p => p.UserId == id))
            .ToListAsync();

        var allUserIds = groups
            .SelectMany(g => g.GroupParticipants.Select(p => p.UserId))
            .Distinct();

        var users = await userRepository.GetUsersByIds(allUserIds);
        var usersDictionary = users.ToDictionary(u => u.Id);

        return Result.Ok(groups.Select(group => new GroupResponse
        {
            Id = group.Id,
            Name = group.Name,
            Participants = group.GroupParticipants.Select(gp =>
            {
                var participant = usersDictionary[gp.UserId];
                return new GroupParticipantResponse
                {
                    UserId = participant.Id,
                    UserName = participant.UserName,
                    Email = participant.Email
                };
            }).ToArray()
        }).ToArray());
}

    public async Task<Result<MeetingResponse[]>> GetUserMeetings(Guid id, Guid requestUserId)
    {
        if (id != requestUserId)
            return Result.Fail(AppError.Forbidden());

        var user = await userRepository.Get(id);
        if (user is null)
            return Result.Fail(AppError.NotFound());

        var meetings = await meetingsRepository.GetAll()
            .Where(meeting => meeting.MeetingParticipants.Any(p => p.UserId == id))
            .ToListAsync();

        var allUserIds = meetings
            .SelectMany(m => m.MeetingParticipants.Select(p => p.UserId))
            .Distinct();

        var users = await userRepository.GetUsersByIds(allUserIds);
        var usersDictionary = users.ToDictionary(u => u.Id);

        return Result.Ok(meetings.Select(meeting => new MeetingResponse
        {
            Id = meeting.Id,
            Name = meeting.Name,
            StartTime = meeting.StartTime,
            EndTime = meeting.EndTime,
            Link = meeting.Link,
            Comment = meeting.Comment,
            Participants = meeting.MeetingParticipants.Select(mp =>
            {
                var participant = usersDictionary[mp.UserId];
                return new MeetingParticipantResponse
                {
                    UserId = participant.Id,
                    UserName = participant.UserName,
                    Email = participant.Email
                };
            }).ToArray()
        }).ToArray());
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
            Slots = (await slotsRepository.GetFilteredByDateTimeSlots(user.Calendar.Id, period.Start,period.End))
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
        if(request.UserIds.All(id => id != requestUserId))
            return Result.Fail(AppError.Forbidden());

        if(request.RequestedPeriod.End<request.RequestedPeriod.Start)
            return Result.Fail(AppError.UnprocessableContent());

        var users = await userRepository.GetUsersByIds(request.UserIds);

        var usersSlots = await Task.WhenAll(
            users.Select(u => slotsRepository.GetFilteredByDateTimeSlots(u.Calendar.Id, request.RequestedPeriod.Start, request.RequestedPeriod.End))
        );

        var slots = usersSlots.SelectMany(x => x)
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
            }).ToArray();

        return Result.Ok(new CalendarResponse
        {
            RequestedPeriod = request.RequestedPeriod,
            Slots = slots
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
            return Result.Fail(AppError.UnprocessableContent("пользователь не найден"));

        var ability = await abilitiesRepository.GetAll().Where(ability => ability.Id == request.AbilityId).FirstOrDefaultAsync();
        if (ability is null)
            return Result.Fail(AppError.UnprocessableContent("доступность не найдена"));

        var slot = new Slot
        {
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Comment = request.Comment,
            Ability = ability,
            CalendarId = user.Calendar.Id,
            Meeting = null
        };
        await slotsRepository.Create(slot);

        await userRepository.UnitOfWork.SaveChangesAsync();

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
            return Result.Fail(AppError.UnprocessableContent("пользователь не найден"));

        var ability = await abilitiesRepository.GetAll().Where(ability => ability.Id == request.AbilityId).FirstOrDefaultAsync();
        if (ability is null)
            return Result.Fail(AppError.UnprocessableContent("доступность не найдена"));

        var slot = await slotsRepository.GetById(id);
        if (slot is null)
            return Result.Fail(AppError.NotFound("слот не найден"));

        slot.StartTime = request.StartTime;
        slot.EndTime = request.EndTime;
        slot.Comment = request.Comment;
        slot.Ability = ability;

        await userRepository.UnitOfWork.SaveChangesAsync();

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

        var slot = await slotsRepository.GetById(id);
        if (slot is null)
            return Result.Fail(AppError.NotFound("слот не найден"));

        slotsRepository.Delete(slot);

        await userRepository.UnitOfWork.SaveChangesAsync();
        
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