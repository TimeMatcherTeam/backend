namespace TimeMatcher.Application.Models.Responses.User;

public record LoginInfoResponse
{
    public required Guid UserId { get; init; }
    public required string AccessToken { get; init; }
}