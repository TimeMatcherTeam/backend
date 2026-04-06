namespace TimeMatcher.Application.Models.Responses.User;

public record UserResponse
{
    public required Guid Id { get; init; }
    public required string NickName { get; init; }
    public required string Email { get; init; }
}