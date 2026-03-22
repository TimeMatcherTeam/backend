using TimeMatcher.Domain.MeetingAggregate;

namespace TimeMatcher.Domain.UserAggregate;

public class Slot
{
    public Guid Id { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public Ability Ability { get; set; }
    public Guid CalendarId { get; init; }
    public Meeting? Meeting { get; set; }
}