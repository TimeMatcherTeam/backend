using FluentResults;
using TimeMatcher.Application.Requests.Group;
using TimeMatcher.Application.Responses.Group;

namespace TimeMatcher.Application.Managers.Groups;

public interface IGroupsManager
{
    Task<Result<GroupResponse>> GetGroupById(Guid id, Guid requestUserId);
    Task<Result<GroupResponse>> CreateGroup(CreateGroupRequest request, Guid requestUserId);
    Task<Result<GroupResponse>> UpdateGroup(Guid id, UpdateGroupRequest request, Guid requestUserId);
    Task<Result> DeleteGroup(Guid id, Guid requestUserId);
    Task<Result<GroupParticipantResponse>> AddParticipant(Guid id, Guid userId, Guid requestUserId);
    Task<Result> DeleteParticipant(Guid id, Guid userId, Guid requestUserId);
}