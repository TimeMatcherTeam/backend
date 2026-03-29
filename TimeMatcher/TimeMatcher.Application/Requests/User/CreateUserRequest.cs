namespace TimeMatcher.Application.Requests.User;

public record CreateUserRequest
{
    public required string UserName { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}