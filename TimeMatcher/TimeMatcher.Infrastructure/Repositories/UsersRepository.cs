using Microsoft.EntityFrameworkCore;
using TimeMatcher.Domain.UserAggregate;

namespace TimeMatcher.Infrastructure.Repositories;

internal class UsersRepository(AppDbContext context): IUsersRepository
{
    public async Task<User?> Get(Guid id)
    {
        return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
    } 

    public IQueryable<User> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<User[]> GetUsersByIds(IEnumerable<Guid> ids)
    {
        throw new NotImplementedException();
    }

    public async Task<Calendar> GetCalendarWithFilteredSlots(Guid userId, DateTime start, DateTime end)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChanges()
    {
        throw new NotImplementedException();
    }
}