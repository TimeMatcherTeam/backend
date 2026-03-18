namespace TimeMatcher.Domain;

public class Participant
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public Guid MeetingId { get; init; }
}