using FluentResults;
using Microsoft.AspNetCore.Identity;
using TimeMatcher.Application.Errors;
using TimeMatcher.Application.Models.Requests.User;
using TimeMatcher.Application.Models.Responses.Group;
using TimeMatcher.Application.Models.Responses.Meeting;
using TimeMatcher.Application.Models.Responses.User;
using TimeMatcher.Domain.Enums;
using TimeMatcher.Domain.UserAggregate;

namespace TimeMatcher.Application.Managers.Users;

internal class UsersManager(
    UserManager<User> userManager, 
    IAccessTokenGenerator accessTokenGenerator, 
    SignInManager<User> signInManager,
    IUsersRepository repository): IUsersManager
{
    public async Task<Result<UserResponse[]>> GetUsers(GetUsersRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<UserResponse>> GetUserById(Guid id, Guid requestUserId)
    {
        var user = await repository.Get(id);
        if (user is null)
            return Result.Fail(AppError.NotFound());
        return Result.Ok(new UserResponse
        {
            Id = user.Id,
            NickName = user.Nickname,
            Email = user.Email,
        });
    }

    public async Task<Result<GroupResponse[]>> GetUserGroups(Guid id, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<MeetingResponse[]>> GetUserMeetings(Guid id, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<CalendarResponse>> GetUserCalendar(Guid id, RequestedPeriod period, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<CalendarResponse>> GetMergedCalendar(GetMergedCalendarRequest request, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<SlotResponse>> CreateSlot(SlotRequest request, Guid userId, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<SlotResponse>> UpdateSlot(Guid id, Guid userId, SlotRequest request, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<UserResponse>> UpdateUser(Guid id, UpdateUserRequest request, Guid requestUserId)
    {
        if (requestUserId != id)
            return Result.Fail(AppError.Forbidden("Нельзя редактировать не себя"));
        var user = await userManager.FindByIdAsync(id.ToString());
        user.Nickname = request.NickName ?? user.Nickname;
        user.Email = request.Email ?? user.Email;
        user.UserName = user.Email;

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return Result.Fail(AppError.UnprocessableContent(
                string.Join(Environment.NewLine, result.Errors.Select(error => error.Description))));
        }

        return Result.Ok(new UserResponse
        {
            Id = user.Id,
            NickName = user.Nickname, 
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
        throw new NotImplementedException();
    }
    
    public async Task<Result<LoginInfoResponse>> RegisterAsync(RegisterUserRequest request)
    {
        var user = new User { Email = request.Email, UserName = request.Email, Nickname = request.NickName };
        
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