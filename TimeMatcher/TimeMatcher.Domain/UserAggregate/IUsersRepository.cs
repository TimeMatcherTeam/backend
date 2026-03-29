namespace TimeMatcher.Domain.UserAggregate;

public interface IUsersRepository
{
    public Task<User> Get(Guid id);
    public Task<IQueryable<User>> GetAll();
    public Task<Calendar> GetCalendar(Guid userId);
    public Task<User> Create(User user);
    public Task Delete(Guid id);
    
}