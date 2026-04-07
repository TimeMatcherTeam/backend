using Microsoft.EntityFrameworkCore;
using TimeMatcher.Domain.AbilityAggregate;
using TimeMatcher.Domain.MeetingAggregate;

namespace TimeMatcher.Domain.UserAggregate;

public class Calendar
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public IReadOnlyList<Slot> Slots => slots.AsReadOnly();
    private List<Slot> slots = [];
    
    public Slot AddSlot(DateTime start, DateTime end, string comment, Ability ability, Meeting? meeting)
    {
        var slot = new Slot
        {
            StartTime = start,
            EndTime = end,
            Comment = comment,
            Ability = ability,
            CalendarId = Id,
            Meeting = meeting
        };
        slots.Add(slot);
        return slot;
    }

    public void RemoveSlot(Guid id)
    {
        var slot = slots.FirstOrDefault(s => s.Id == id);
        slots.Remove(slot);
    }
}