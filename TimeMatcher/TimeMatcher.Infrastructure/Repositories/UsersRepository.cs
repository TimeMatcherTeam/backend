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
            .FirstOrDefaultAsync(u => u.Id == id);
    } 

    public IQueryable<User> GetAll()
    {
        return context.Users
            .Include(u => u.Calendar);
    }

    public async Task<User[]> GetUsersByIds(IEnumerable<Guid> ids)
    {
        return await context.Users
            .Include(u => u.Calendar)
            .Where(u => ids.Contains(u.Id))
            .ToArrayAsync();
    }

    public IUnitOfWork UnitOfWork => context;
}