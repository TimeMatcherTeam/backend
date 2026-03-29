namespace TimeMatcher.Application.Responses.Meeting;

public record MeetingResponse
{
    public required Guid Id { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
    public required string Name { get; init; }
    public required string? Comment { get; init; }
    public required string? Link { get; init; }
    public required MeetingParticipantResponse[] Participants { get; init; }
}