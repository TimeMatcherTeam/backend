namespace TimeMatcher.Application.Responses.User;

public record SlotResponse
{
    public required Guid Id { get; init; }
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
    public AbilityResponse Ability { get; set; }
}