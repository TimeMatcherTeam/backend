using TimeMatcher.Domain.MeetingAggregate;

namespace TimeMatcher.Infrastructure.Persistence.Repositories;

public class MeetingsRepository:IMeetingsRepository
{
    public async Task<Meeting> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Meeting> Create(Meeting meeting)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Meeting> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task SaveChanges()
    {
        throw new NotImplementedException();
    }
}