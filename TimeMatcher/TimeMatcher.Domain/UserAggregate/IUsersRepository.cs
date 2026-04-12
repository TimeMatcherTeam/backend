namespace TimeMatcher.Domain.UserAggregate;

public interface IUsersRepository: IRepository
{
    public Task<User?> Get(Guid id);
    public IQueryable<User> GetAll();
    public Task<User[]> GetUsersByIds(IEnumerable<Guid> ids);
    public Task<Calendar> GetCalendarWithFilteredSlots(Guid userId, DateTime start, DateTime end);
}