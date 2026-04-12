using System.ComponentModel.DataAnnotations;

namespace TimeMatcher.Application.Models.Requests.User;

public record GetUsersRequest
{
    public required string SearchText { get; init; }
    public required int Limit { get; init; }
    public required int Page { get; init; }
}