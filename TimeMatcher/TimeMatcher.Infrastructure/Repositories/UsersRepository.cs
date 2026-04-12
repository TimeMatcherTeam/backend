using Microsoft.EntityFrameworkCore;
using TimeMatcher.Domain;
using TimeMatcher.Domain.UserAggregate;

namespace TimeMatcher.Infrastructure.Repositories;

internal class UsersRepository(AppDbContext context): IUsersRepository
{
    public async Task<User?> Get(Guid id)
    {
        return await context.Users
            .Include(u => u.Calendar)
                .ThenInclude(c => c.Slots)
                    .ThenInclude(s => s.Ability)
            .Include(u => u.Calendar)
                .ThenInclude(c => c.Slots)
                    .ThenInclude(s => s.Meeting)
            .FirstOrDefaultAsync(u => u.Id == id);
    } 

    public IQueryable<User> GetAll()
    {
        return context.Users
            .Include(u => u.Calendar)
                .ThenInclude(c => c.Slots)
                    .ThenInclude(s => s.Ability)
            .Include(u => u.Calendar)
                .ThenInclude(c => c.Slots)
                    .ThenInclude(s => s.Meeting);

    }

    public async Task<User[]> GetUsersByIds(IEnumerable<Guid> ids)
    {
        return await context.Users
            .Include(u => u.Calendar)
                .ThenInclude(c => c.Slots)
                    .ThenInclude(s => s.Ability)
            .Include(u => u.Calendar)
                .ThenInclude(c => c.Slots)
                    .ThenInclude(s => s.Meeting)
            .Where(u => ids.Contains(u.Id))
            .ToArrayAsync();
    }

    public async Task<Calendar> GetCalendarWithFilteredSlots(Guid userId, DateTime start, DateTime end)
    {
        return await context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Calendar)
            .Include(c => c.Slots.Where(s => s.EndTime >= start && s.StartTime <= end))
            .FirstOrDefaultAsync();
    }

    public IUnitOfWork UnitOfWork => context;
}