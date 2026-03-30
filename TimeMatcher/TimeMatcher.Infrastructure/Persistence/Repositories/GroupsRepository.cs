using TimeMatcher.Domain.GroupAggregate;

namespace TimeMatcher.Infrastructure.Persistence.Repositories;

public class GroupsRepository: IGroupsRepository
{
    public async Task<Group> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public IQueryable<Group> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task<Group> Create(Group group)
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