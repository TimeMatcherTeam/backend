using System.ComponentModel.DataAnnotations;

namespace TimeMatcher.Application.Models.Requests.User;

public record RegisterUserRequest
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Никнейм обязателен")]
    public required string NickName { get; init; }
    [EmailAddress]
    public required string Email { get; init; }
    public required string Password { get; init; }
}