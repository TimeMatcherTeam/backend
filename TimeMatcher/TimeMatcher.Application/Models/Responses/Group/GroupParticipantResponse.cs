using TimeMatcher.Domain.Enums;

namespace TimeMatcher.Application.Models.Responses.Group;

public record GroupParticipantResponse
{
    public required Guid UserId { get; init; }
    public required string UserName { get; init; }
    public required string Email { get; init; }
    public required Role Role { get; init; }
}