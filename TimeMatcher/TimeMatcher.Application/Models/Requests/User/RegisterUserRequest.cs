using System.ComponentModel.DataAnnotations;

namespace TimeMatcher.Application.Models.Requests.User;

public record RegisterUserRequest
{
    public required string UserName { get; init; }
    [EmailAddress]
    public required string Email { get; init; }
    public required string Password { get; init; }
}