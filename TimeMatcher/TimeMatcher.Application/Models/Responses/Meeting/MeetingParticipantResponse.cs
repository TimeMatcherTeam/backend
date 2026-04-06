namespace TimeMatcher.Application.Models.Responses.Meeting;

public record MeetingParticipantResponse
{
    public required Guid UserId { get; init; }
    public required string NickName { get; init; }
    public required string Email { get; init; }
}