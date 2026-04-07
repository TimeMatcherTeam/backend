using System.ComponentModel.DataAnnotations;

namespace TimeMatcher.Application.Models.Requests.User;

public record UpdateUserRequest
{
    public required string UserName { get; init; }
    [EmailAddress]
    public required string Email { get; init; }
}