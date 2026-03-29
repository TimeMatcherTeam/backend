namespace TimeMatcher.Application.Requests.Meeting;

public record UpdateMeetingRequest
{
    public required string Name { get; init; }
    public required string? Comment { get; init; }
    public required bool IsOnline { get; init; }
}