using TimeMatcher.Application.Models.Requests.User;

namespace TimeMatcher.Application.Models.Responses.User;

public record MergedCalendarResponse
{
    public required MergedSlotResponse[] Slots { get; init; }
    public required RequestedPeriod RequestedPeriod { get; init; }
}