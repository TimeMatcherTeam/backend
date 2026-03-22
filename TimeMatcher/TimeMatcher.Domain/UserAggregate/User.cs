namespace TimeMatcher.Domain.UserAggregate;

public class User
{
    public Guid Id { get; init; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public Calendar Calendar { get; init; } = new();
}