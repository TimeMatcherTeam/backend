namespace TimeMatcher.Application.Requests.User;

public record GetMergedCalendarRequest
{
    public required Guid[] Users { get; init; }
}