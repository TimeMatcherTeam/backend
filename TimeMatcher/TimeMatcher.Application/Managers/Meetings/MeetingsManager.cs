using FluentResults;
using TimeMatcher.Application.Requests.Meeting;
using TimeMatcher.Application.Responses.Meeting;

namespace TimeMatcher.Application.Managers.Meetings;

public class MeetingsManager: IMeetingsManager
{
    public async Task<Result<MeetingResponse>> GetMeetingById(Guid id, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<MeetingResponse>> CreateMeeting(CreateMeetingRequest request, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<MeetingResponse>> UpdateMeeting(Guid id, UpdateMeetingRequest request, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteMeeting(Guid id, Guid requestUserId)
    {
        throw new NotImplementedException();
    }
}