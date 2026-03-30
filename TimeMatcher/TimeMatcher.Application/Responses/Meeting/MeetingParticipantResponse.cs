using TimeMatcher.Domain.Enums;

namespace TimeMatcher.Application.Responses.Meeting;

public record MeetingParticipantResponse
{
    public required Guid UserId { get; init; }
    public required string UserName { get; init; }
    public required string Email { get; init; }
}