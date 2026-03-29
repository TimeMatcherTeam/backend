namespace TimeMatcher.Application.Responses.User;

public record CalendarResponse
{
    public required SlotResponse[] Slots { get; init; }
}