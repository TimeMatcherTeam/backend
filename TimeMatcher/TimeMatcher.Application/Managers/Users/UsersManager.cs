using FluentResults;
using TimeMatcher.Application.Requests.User;
using TimeMatcher.Application.Responses.Group;
using TimeMatcher.Application.Responses.Meeting;
using TimeMatcher.Application.Responses.User;

namespace TimeMatcher.Application.Managers.Users;

public class UsersManager: IUsersManager
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

    public async Task<Result<UserResponse>> CreateUser(CreateUserRequest request)
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
}