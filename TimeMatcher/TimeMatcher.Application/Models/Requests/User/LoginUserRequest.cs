namespace TimeMatcher.Application.Models.Requests.User;

public class LoginUserRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}