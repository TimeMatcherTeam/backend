namespace TimeMatcher.Domain.UserAggregate;

public interface IUsersRepository
{
    public Task<User?> Get(Guid id);
    public IQueryable<User> GetAll();
    public Task<User[]> GetUsersByIds(IEnumerable<Guid> ids);
    public Task<Calendar> GetCalendar(Guid userId);
    public Task Delete(Guid id);
    public Task SaveChanges();
}