namespace TimeMatcher.Domain.UserAggregate;

public class Calendar
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public IReadOnlyList<Slot> Slots => slots.AsReadOnly();
    private List<Slot> slots = [];
}