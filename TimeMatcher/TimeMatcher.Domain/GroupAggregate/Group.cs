namespace TimeMatcher.Domain.GroupAggregate;

public class Group
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public IReadOnlyList<GroupParticipant> GroupParticipants => groupParticipants.AsReadOnly();
    private List<GroupParticipant> groupParticipants = [];
}