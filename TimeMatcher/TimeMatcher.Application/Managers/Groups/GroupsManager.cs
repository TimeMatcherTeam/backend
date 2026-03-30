using FluentResults;
using Microsoft.EntityFrameworkCore;
using TimeMatcher.Application.Errors;
using TimeMatcher.Application.Requests.Group;
using TimeMatcher.Application.Responses.Group;
using TimeMatcher.Domain.Enums;
using TimeMatcher.Domain.GroupAggregate;
using TimeMatcher.Domain.UserAggregate;

namespace TimeMatcher.Application.Managers.Groups;

public class GroupsManager(IGroupsRepository groupsRepository, IUsersRepository usersRepository): IGroupsManager
{
    
    public async Task<Result<GroupResponse>> GetGroupById(Guid id, Guid requestUserId)
    {
        var group = await groupsRepository.Get(id);
        if (group is null) return Result.Fail(AppError.NotFound());
        if (!group.GroupParticipants.Any(gp => gp.UserId == requestUserId)) return Result.Fail(AppError.Forbidden());
        var userIds = group.GroupParticipants.Select(gp => gp.UserId);
        var users = await usersRepository.GetAll()
            .Where(x => userIds.Contains(x.Id))
            .ToDictionaryAsync(u => u.Id);
        return Result.Ok(new GroupResponse
        {
            Id = group.Id,
            Name = group.Name,
            Participants = group.GroupParticipants.Select(gp =>
            {
                var user = users[gp.UserId];
                return new GroupParticipantResponse
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                };
            }).ToArray()
        });
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