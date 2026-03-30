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

    public async Task<Calendar> GetCalendar(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<User> Create(User user)
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