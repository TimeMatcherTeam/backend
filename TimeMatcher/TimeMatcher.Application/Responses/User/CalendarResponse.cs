using TimeMatcher.Application.Requests.User;

namespace TimeMatcher.Application.Responses.User;

public record CalendarResponse
{
    public required SlotResponse[] Slots { get; init; }
    public required RequestedPeriod RequestedPeriod { get; init; }
}