namespace TimeMatcher.Domain.GroupAggregate;

public interface IGroupRepository
{
    public Group GetGroupById(Guid groupId);
}