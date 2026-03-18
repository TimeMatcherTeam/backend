namespace TimeMatcher.Domain;

public class Calendar
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public ICollection<Slot> Slots { get; init; } = new List<Slot>();
}