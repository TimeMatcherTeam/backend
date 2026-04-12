using Microsoft.EntityFrameworkCore;
using TimeMatcher.Domain;
using TimeMatcher.Domain.UserAggregate;

namespace TimeMatcher.Infrastructure.Repositories;

internal class SlotsRepository(AppDbContext context): ISlotsRepository
{
    public IUnitOfWork UnitOfWork => context;
    public async Task<Slot?> GetById(Guid slotId)
    {
        return await context.Slots
            .Include(s => s.Ability)
            .Include(s => s.Meeting)
            .FirstOrDefaultAsync(s => s.Id == slotId);
    }

    public Task<Slot[]> GetFilteredByDateTimeSlots(Guid calendarId, DateTime start, DateTime end)
    {
        return context.Slots
            .Where(s => s.StartTime < end && s.EndTime > start && s.CalendarId == calendarId)
            .Include(s => s.Ability)
            .Include(s => s.Meeting)
            .ToArrayAsync();
    }

    public async Task<Slot[]> GetFilteredByDateTimeSlotsManyCalendars(Guid[] calendarId, DateTime start, DateTime end)
    {
        return await context.Slots
            .Include(s => s.Ability)
            .Include(s => s.Meeting)
            .Where(s => calendarId.Contains(s.CalendarId) && s.StartTime < end && s.EndTime > start)
            .ToArrayAsync();
    }

    public async Task<Slot> Create(Slot slot)
    {
        await context.Slots.AddAsync(slot);
        return slot;
    }

    public void Delete(Slot slot)
    {
        context.Slots.Remove(slot);
    }
}