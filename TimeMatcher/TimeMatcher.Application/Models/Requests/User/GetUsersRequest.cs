using System.ComponentModel.DataAnnotations;

namespace TimeMatcher.Application.Models.Requests.User;

public record GetUsersRequest
{
    [EmailAddress]
    public required string? Email { get; init; }
    public required string? NickName { get; init; }
    public required int Limit { get; init; }
    public required int Page { get; init; }
}