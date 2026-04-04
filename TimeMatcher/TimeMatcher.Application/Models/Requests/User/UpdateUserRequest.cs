namespace TimeMatcher.Application.Models.Requests.User;

public record UpdateUserRequest
{
    public required string UserName { get; init; }
    public required string Email { get; init; }
}