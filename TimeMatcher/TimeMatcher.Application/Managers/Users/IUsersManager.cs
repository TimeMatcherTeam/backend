using FluentResults;
using TimeMatcher.Application.Requests.User;
using TimeMatcher.Application.Responses.Group;
using TimeMatcher.Application.Responses.Meeting;
using TimeMatcher.Application.Responses.User;

namespace TimeMatcher.Application.Managers.Users;

public interface IUsersManager
{
   Task<Result<UserResponse[]>> GetUsers(GetUsersRequest request, Guid requestUserId);
   Task<Result<UserResponse>> GetUserById(Guid id, Guid requestUserId);
   Task<Result<GroupResponse[]>> GetUserGroups(Guid id, Guid requestUserId);
   Task<Result<MeetingResponse[]>> GetUserMeetings(Guid id, Guid requestUserId);
   Task<Result<CalendarResponse>> GetUserCalendar(Guid id, RequestedPeriod period, Guid requestUserId);
   Task<Result<CalendarResponse>> GetMergedCalendar(GetMergedCalendarRequest request, Guid requestUserId);
   Task<Result<UserResponse>> CreateUser(CreateUserRequest request);
   Task<Result<SlotResponse>> CreateSlot(SlotRequest request, Guid userId,  Guid requestUserId);
   Task<Result<SlotResponse>> UpdateSlot(Guid id, Guid userId, SlotRequest request, Guid requestUserId);
   Task<Result<UserResponse>> UpdateUser(Guid id, UpdateUserRequest request, Guid requestUserId);
   Task<Result> DeleteUser(Guid id, Guid requestUserId);
   Task<Result> DeleteSlot(Guid id, Guid userId, Guid requestUserId);
   
   
}