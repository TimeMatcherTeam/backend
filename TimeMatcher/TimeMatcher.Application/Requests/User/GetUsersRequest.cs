namespace TimeMatcher.Application.Requests.User;

public record GetUsersRequest
{
    public required string? Email { get; init; }
    public required string? UserName { get; init; }
    public required int Limit { get; init; }
    public required int Page { get; init; }
}