namespace TimeMatcher.Application.Requests.Group;

public record CreateGroupRequest
{
    public required string Name { get; init; }
    public required Guid[] Participants { get; init; }
}