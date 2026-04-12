namespace TimeMatcher.Application.Models.Requests.User;

public class SlotRequest
{    
    public required DateTime StartTime { get; init; }
    public required DateTime EndTime { get; init; }
    public required string? Title { get; init; }
    public required Guid AbilityId { get; init; }
}