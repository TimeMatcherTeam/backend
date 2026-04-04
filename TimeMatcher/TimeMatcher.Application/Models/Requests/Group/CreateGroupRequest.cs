namespace TimeMatcher.Application.Models.Requests.Group;

public record CreateGroupRequest
{
    public required string Name { get; init; }
    public required Guid[] ParticipantIds { get; init; }
}