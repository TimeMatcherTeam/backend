using TimeMatcher.Domain.UserAggregate;

namespace TimeMatcher.Infrastructure.Persistence.Repositories;

public class UsersRepository: IUsersRepository
{
    public async Task<User> Get(Guid id)
    {
        throw new NotImplementedException();
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

    public async Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChanges()
    {
        throw new NotImplementedException();
    }
}