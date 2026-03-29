using FluentResults;
using TimeMatcher.Application.Requests.Group;
using TimeMatcher.Application.Responses.Group;

namespace TimeMatcher.Application.Managers.Groups;

public class GroupsManager: IGroupsManager
{
    public async Task<Result<GroupResponse>> GetGroupById(Guid id, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<GroupResponse>> CreateGroup(CreateGroupRequest request, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<GroupResponse>> UpdateGroup(Guid id, UpdateGroupRequest request, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteGroup(Guid id, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<GroupParticipantResponse>> AddParticipant(Guid id, Guid userId, Guid requestUserId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteParticipant(Guid id, Guid userId, Guid requestUserId)
    {
        throw new NotImplementedException();
    }
}