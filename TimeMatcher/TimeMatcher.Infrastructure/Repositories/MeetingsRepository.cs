using Microsoft.EntityFrameworkCore;
using TimeMatcher.Domain.MeetingAggregate;

namespace TimeMatcher.Infrastructure.Repositories;

internal class MeetingsRepository(AppDbContext context) : IMeetingsRepository
{
    public async Task<Meeting?> Get(Guid id)
    {
        return await GetAll()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Meeting> Create(Meeting meeting)
    {
        await context.Meetings.AddAsync(meeting);
        return meeting;
    }

    public void Delete(Meeting meeting)
    {
        context.Meetings.Remove(meeting);
    }

    public IQueryable<Meeting> GetAll()
    {
        return context.Meetings.Include(m => m.MeetingParticipants);
    }

    public async Task SaveChanges()
    {
        await context.SaveChangesAsync();
    }
}