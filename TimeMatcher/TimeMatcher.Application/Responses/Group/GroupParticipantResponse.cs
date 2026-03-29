using TimeMatcher.Domain.Enums;

namespace TimeMatcher.Application.Responses.Group;

public record GroupParticipantResponse
{
    public required Guid userId { get; init; }
    public required string UserName { get; init; }
    public required string Email { get; init; }
}