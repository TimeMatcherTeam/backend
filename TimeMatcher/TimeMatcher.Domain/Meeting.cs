namespace TimeMatcher.Domain;

public class Meeting
{
    public Guid Id { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string Name { get; init; }
    public string Comment { get; init; }
    public string Address { get; init; }
    public bool isOnline { get; init; }
    public ICollection<Participant> Participants { get; init; } = new List<Participant>();
}