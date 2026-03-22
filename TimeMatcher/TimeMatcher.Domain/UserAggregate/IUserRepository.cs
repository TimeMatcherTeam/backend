namespace TimeMatcher.Domain.UserAggregate;

public interface IUserRepository
{
    public User GetUserById(Guid userId);
}