using FluentResults;
using TimeMatcher.Application.Models.Requests.User;
using TimeMatcher.Application.Models.Responses.Group;
using TimeMatcher.Application.Models.Responses.Meeting;
using TimeMatcher.Application.Models.Responses.User;

namespace TimeMatcher.Application.Managers.Users;

public interface IUsersManager
{
   Task<Result<UserResponse[]>> GetUsers(GetUsersRequest request);
   Task<Result<UserResponse>> GetUserById(Guid id, Guid requestUserId);
   Task<Result<GroupResponse[]>> GetUserGroups(Guid id, Guid requestUserId);
   Task<Result<MeetingResponse[]>> GetUserMeetings(Guid id, Guid requestUserId);
   Task<Result<CalendarResponse>> GetUserCalendar(Guid id, RequestedPeriod period, Guid requestUserId);
   Task<Result<CalendarResponse>> GetMergedCalendar(GetMergedCalendarRequest request, Guid requestUserId);
   Task<Result<SlotResponse>> CreateSlot(SlotRequest request, Guid userId,  Guid requestUserId);
   Task<Result<SlotResponse>> UpdateSlot(Guid id, Guid userId, SlotRequest request, Guid requestUserId);
   Task<Result<UserResponse>> UpdateUser(Guid id, UpdateUserRequest request, Guid requestUserId);
   Task<Result> DeleteUser(Guid id, Guid requestUserId);
   Task<Result> DeleteSlot(Guid id, Guid userId, Guid requestUserId);
   Task<Result<LoginInfoResponse>> RegisterAsync(RegisterUserRequest request);
   Task<Result<LoginInfoResponse>> LoginAsync(string email, string password);
   Task<Result<LoginInfoResponse>> ChangePassword(Guid userId, ChangePasswordRequest request, Guid requestUserId);
   
}