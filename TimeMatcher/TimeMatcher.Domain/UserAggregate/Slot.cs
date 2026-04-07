using TimeMatcher.Domain.AbilityAggregate;
using TimeMatcher.Domain.MeetingAggregate;

namespace TimeMatcher.Domain.UserAggregate;

public class Slot
{
    public Guid Id { get; init; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Comment { get; set; }
    public Ability Ability { get; set; }
    public Guid CalendarId { get; init; }
    public Meeting? Meeting { get; set; }
}