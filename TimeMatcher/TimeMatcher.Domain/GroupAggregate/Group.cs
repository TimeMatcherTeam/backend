using TimeMatcher.Domain.Enums;

namespace TimeMatcher.Domain.GroupAggregate;

public class Group
{
    public Guid Id { get; init; }
    public string Name { get; set; }
    public IReadOnlyList<GroupParticipant> GroupParticipants => groupParticipants.AsReadOnly();
    private List<GroupParticipant> groupParticipants = [];
    public void AddParticipant(Guid userId, Role role)
    {
        groupParticipants.Add(new GroupParticipant
        {
            UserId = userId, 
            GroupId = Id,
            Role = role
        });
    }

    public void RemoveParticipant(Guid userId)
    {
        var groupParticipant = groupParticipants.FirstOrDefault(g => g.UserId == userId);
        groupParticipants.Remove(groupParticipant);
    }
}