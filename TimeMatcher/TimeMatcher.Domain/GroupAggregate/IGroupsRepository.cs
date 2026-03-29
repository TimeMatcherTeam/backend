namespace TimeMatcher.Domain.GroupAggregate;

public interface IGroupsRepository
{
    public Task<Group> Get(Guid id);
    public Task<Group> Create(Group group);
    public Task Delete(Guid id);
    public Task<Group[]> GetUserGroups(Guid userId);
    public Task SaveChanges();
}