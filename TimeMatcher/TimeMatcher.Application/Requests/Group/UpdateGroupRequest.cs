namespace TimeMatcher.Application.Requests.Group;

public record UpdateGroupRequest
{
    public required string Name { get; init; }
}