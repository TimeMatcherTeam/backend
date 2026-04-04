using Microsoft.EntityFrameworkCore;
using TimeMatcher.Domain.MeetingAggregate;

namespace TimeMatcher.Infrastructure.Repositories;

public class MeetingsRepository:IMeetingsRepository
{
    private readonly AppDbContext _context;

    public MeetingsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Meeting?> Get(Guid id)
    {
        return await _context.Meetings.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Meeting> Create(Meeting meeting)
    {
        await _context.Meetings.AddAsync(meeting);
        return meeting;
    }

    public void Delete(Meeting meeting)
    {
        _context.Meetings.Remove(meeting);
    }

    public IQueryable<Meeting> GetAll()
    {
        return _context.Meetings.AsQueryable();
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}