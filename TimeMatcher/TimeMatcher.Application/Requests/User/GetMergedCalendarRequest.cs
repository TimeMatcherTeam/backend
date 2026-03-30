namespace TimeMatcher.Application.Requests.User;

public record GetMergedCalendarRequest
{
    public required Guid[] UserIds { get; init; }
    public required RequestedPeriod RequestedPeriod { get; init; }
}