namespace TimeMatcher.Application.Models.Responses.Meeting;

public record MeetingParticipantResponse
{
    public required Guid UserId { get; init; }
    public required string UserName { get; init; }
    public required string Email { get; init; }
}