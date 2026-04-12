namespace TimeMatcher.Domain.GroupAggregate;

public interface IGroupsRepository: IRepository
{
    public Task<Group?> Get(Guid id);
    public Task<Group> Create(Group group);
    public void Delete(Group group);
    public IQueryable<Group> GetAll();
}