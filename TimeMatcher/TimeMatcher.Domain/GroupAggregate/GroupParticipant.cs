using TimeMatcher.Domain.Enums;

namespace TimeMatcher.Domain.GroupAggregate;

public class GroupParticipant
{
    public Guid Id { get; init; }
    public Guid GroupId { get; init; }
    public Guid UserId { get; init; }
    public Role Role { get; set; }
}