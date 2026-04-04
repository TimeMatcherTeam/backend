using FluentResults;
using TimeMatcher.Application.Models.Requests.Meeting;
using TimeMatcher.Application.Models.Responses.Meeting;

namespace TimeMatcher.Application.Managers.Meetings;

public interface IMeetingsManager
{
    Task<Result<MeetingResponse>> GetMeetingById(Guid id, Guid requestUserId);
    Task<Result<MeetingResponse>> CreateMeeting(CreateMeetingRequest request, Guid requestUserId);
    Task<Result<MeetingResponse>> UpdateMeeting(Guid id, UpdateMeetingRequest request, Guid requestUserId);
    Task<Result> DeleteMeeting(Guid id, Guid requestUserId);
}