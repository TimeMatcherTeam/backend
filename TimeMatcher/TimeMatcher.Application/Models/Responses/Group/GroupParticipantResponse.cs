namespace TimeMatcher.Application.Models.Responses.Group;

public record GroupParticipantResponse
{
    public required Guid UserId { get; init; }
    public required string NickName { get; init; }
    public required string Email { get; init; }
}