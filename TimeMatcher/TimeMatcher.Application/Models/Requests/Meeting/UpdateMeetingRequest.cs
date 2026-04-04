namespace TimeMatcher.Application.Models.Requests.Meeting;

public record UpdateMeetingRequest
{
    public required string Name { get; init; }
    public required string? Comment { get; init; }
}