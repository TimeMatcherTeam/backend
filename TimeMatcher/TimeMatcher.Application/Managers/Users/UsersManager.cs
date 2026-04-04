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

public class UsersManager(UserManager<User> userManager, IAccessTokenGenerator accessTokenGenerator, SignInManager<User> signInManager): IUsersManager
{
    public async Task<Result<UserResponse[]>> GetUsers(GetUsersRequest request, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<UserResponse>> GetUserById(Guid id, Guid requestUserId)
    {
        throw new NotImplementedException();
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

    public async Task<Result<UserResponse>> CreateUser(RegisterUserRequest request)
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
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteUser(Guid id, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteSlot(Guid id, Guid userId, Guid requestUserId)
    {
        throw new NotImplementedException();
    }
    
    public async Task<Result<LoginInfoResponse>> RegisterAsync(RegisterUserRequest request)
    {
        var user = new User { Email = request.Email, UserName = request.UserName };
        
        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Result.Fail(AppError.Validation(
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
}