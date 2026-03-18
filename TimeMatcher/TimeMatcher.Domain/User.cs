namespace TimeMatcher.Domain;

public class User
{
    public Guid Id { get; init; }
    public FullName FullName { get; set; }
    public string Email { get; set; }
    public Calendar Calendar { get; init; } = new Calendar();
}