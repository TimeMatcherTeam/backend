namespace TimeMatcher.Domain.MeetingAggregate;

public interface IMeetingsRepository
{
    public Task<Meeting?> Get(Guid id);
    public Task<Meeting> Create(Meeting meeting);
    public void Delete(Meeting meeting);
    public IQueryable<Meeting> GetAll();
    public Task SaveChanges();
}