using TimeMatcher.Domain.Enums;

namespace TimeMatcher.Domain.MeetingAggregate;

public class MeetingParticipant
{
    public Guid UserId { get; init; }
    public Guid MeetingId { get; init; }
    public Role Role { get; init; }
}