using System.ComponentModel.DataAnnotations;

namespace TimeMatcher.Application.Models.Requests.User;

public class LoginUserRequest
{
    [EmailAddress]
    public required string Email { get; init; }
    public required string Password { get; init; }
}