namespace TimeMatcher.Application.Requests.Meeting;

public record CreateMeetingRequest
{
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
    public required string Name { get; init; }
    public required string? Comment { get; init; }
    public required bool IsOnline { get; init; }
    public required Guid[] ParticipantIds { get; init; }
}