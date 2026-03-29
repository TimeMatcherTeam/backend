using TimeMatcher.Domain.AbilityAggregate;
using TimeMatcher.Domain.MeetingAggregate;

namespace TimeMatcher.Domain.UserAggregate;

public class Calendar
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public IReadOnlyList<Slot> Slots => slots.AsReadOnly();
    private List<Slot> slots = [];
    
    public void AddSlot(DateTime start, DateTime end, Ability ability, Meeting? meeting)
    {
        slots.Add(new Slot
        {
            StartTime = start,
            EndTime = end,
            Ability = ability,
            CalendarId = Id,
            Meeting = meeting
        });
    }

    public void RemoveSlot(Guid id)
    {
        var slot = slots.FirstOrDefault(s => s.Id == id);
        slots.Remove(slot);
    }
}