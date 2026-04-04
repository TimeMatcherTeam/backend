namespace TimeMatcher.Application.Models.Requests.Group;

public record UpdateGroupRequest
{
    public required string Name { get; init; }
}