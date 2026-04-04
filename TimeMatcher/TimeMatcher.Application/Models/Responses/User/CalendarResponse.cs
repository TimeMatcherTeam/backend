using TimeMatcher.Application.Models.Requests.User;

namespace TimeMatcher.Application.Models.Responses.User;

public record CalendarResponse
{
    public required SlotResponse[] Slots { get; init; }
    public required RequestedPeriod RequestedPeriod { get; init; }
}