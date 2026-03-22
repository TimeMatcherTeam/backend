namespace TimeMatcher.Domain.MeetingAggregate;

public interface IMeetingRepository
{
    public Meeting GetMeetingById(Guid meetingId);
}