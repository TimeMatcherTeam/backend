using System.ComponentModel.DataAnnotations;

namespace TimeMatcher.Application.Models.Requests.User;

public record GetUsersRequest
{
    [EmailAddress]
    public required string? Email { get; init; }//todo maybe frontend dont know how to understand what user give him
    public required string? UserName { get; init; }
    public required int Limit { get; init; }
    public required int Page { get; init; }
}