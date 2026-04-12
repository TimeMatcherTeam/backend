namespace TimeMatcher.Application.Models.Responses.User;

public record MergedSlotResponse
{
    public required Guid Id { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
    public required AbilityResponse Ability { get; init; }
}