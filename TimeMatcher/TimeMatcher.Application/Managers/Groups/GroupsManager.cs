using FluentResults;
using TimeMatcher.Application.Errors;
using TimeMatcher.Application.Models.Requests.Group;
using TimeMatcher.Application.Models.Responses.Group;
using TimeMatcher.Domain.Enums;
using TimeMatcher.Domain.GroupAggregate;
using TimeMatcher.Domain.UserAggregate;

namespace TimeMatcher.Application.Managers.Groups;

internal class GroupsManager(
    IGroupsRepository groupsRepository, 
    IUsersRepository usersRepository
    ): IGroupsManager
{
    
    public async Task<Result<GroupResponse>> GetGroupById(Guid id, Guid requestUserId)
    {
        var group = await groupsRepository.Get(id);
        if (group is null) return Result.Fail(AppError.NotFound());
        if (group.GroupParticipants.All(gp => gp.UserId != requestUserId)) 
            return Result.Fail(AppError.Forbidden());
        var userIds = group.GroupParticipants.Select(gp => gp.UserId);
        var users = await usersRepository.GetUsersByIds(userIds);
        var usersDictionary = users.ToDictionary(u => u.Id);
        return Result.Ok(new GroupResponse
        {
            Id = group.Id,
            Name = group.Name,
            Participants = group.GroupParticipants.Select(gp =>
            {
                var user = usersDictionary[gp.UserId];
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
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Fail(AppError.UnprocessableContent("Название не может быть пустым"));
        if(request.ParticipantIds.Length <= 1) 
            return Result.Fail(AppError.UnprocessableContent("Необходимо хотя бы 2 участника"));

        var group = new Group
        {
            Id = Guid.NewGuid(),
            Name = request.Name
        };
        var users = await usersRepository.GetUsersByIds(request.ParticipantIds);
        if (users.Length != request.ParticipantIds.Length)
            return Result.Fail(AppError.UnprocessableContent("Все участники должны существовать"));
        var usersDictionary = users.ToDictionary(u => u.Id);
        foreach (var user in users)
        {
            group.AddParticipant(user.Id, user.Id == requestUserId ? Role.Organizer : Role.Participant);
        }

        await groupsRepository.Create(group);
        await groupsRepository.UnitOfWork.SaveChangesAsync();

        return Result.Ok(new GroupResponse
        {
            Id = group.Id,
            Name = group.Name,
            Participants = group.GroupParticipants.Select(gp =>
            {
                var user = usersDictionary[gp.UserId];
                return new GroupParticipantResponse
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                };
            }).ToArray()
        });
    }

    public async Task<Result<GroupResponse>> UpdateGroup(Guid id, UpdateGroupRequest request, Guid requestUserId)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Fail(AppError.UnprocessableContent("Название не может быть пустым"));

        var group = await groupsRepository.Get(id);
        if (group is null) 
            return Result.Fail(AppError.NotFound());

        var requestUser = group.GroupParticipants.FirstOrDefault(gp => gp.UserId == requestUserId);
        if (requestUser is not { Role: Role.Organizer }) 
            return Result.Fail(AppError.Forbidden());

        group.Name = request.Name;
        await groupsRepository.UnitOfWork.SaveChangesAsync();

        var usersIds = group.GroupParticipants.Select(m => m.UserId);
        var users = await usersRepository.GetUsersByIds(usersIds);
        var usersDictionary = users.ToDictionary(u => u.Id);
        return Result.Ok(new GroupResponse
        {
            Id = group.Id,
            Name = group.Name,
            Participants = group.GroupParticipants.Select(gp =>
            {
                var user = usersDictionary[gp.UserId];
                return new GroupParticipantResponse
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                };
            }).ToArray()
        });
    }

    public async Task<Result> DeleteGroup(Guid id, Guid requestUserId)
    {
        var group = await groupsRepository.Get(id);
        if (group is null) 
            return Result.Fail(AppError.NotFound());

        var requestUser = group.GroupParticipants.FirstOrDefault(gp => gp.UserId == requestUserId);
        if (requestUser is not { Role: Role.Organizer })
            return Result.Fail(AppError.Forbidden());

        groupsRepository.Delete(group);
        await groupsRepository.UnitOfWork.SaveChangesAsync();
        return Result.Ok();
    }

    public async Task<Result<GroupParticipantResponse>> AddParticipant(Guid id, Guid userId, Guid requestUserId)
    {
        var group = await groupsRepository.Get(id);
        if (group is null) 
            return Result.Fail(AppError.NotFound("Группа не найдена"));

        var requestUser = group.GroupParticipants.FirstOrDefault(gp => gp.UserId == requestUserId);
        if (requestUser is not { Role: Role.Organizer })
            return Result.Fail(AppError.Forbidden());
        
        var user = await usersRepository.Get(userId);
        if(user is null)
            return Result.Fail(AppError.UnprocessableContent("Человек не найден"));

        if (group.GroupParticipants.Any(p=> p.UserId == userId))
            return Result.Fail(AppError.Conflict());

        group.AddParticipant(userId,Role.Participant);
        await groupsRepository.UnitOfWork.SaveChangesAsync();

        return Result.Ok(new GroupParticipantResponse
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email
        });
    }

    public async Task<Result> DeleteParticipant(Guid id, Guid userId, Guid requestUserId)
    {
        var group = await groupsRepository.Get(id);
        if (group is null) 
            return Result.Fail(AppError.NotFound("Группа не найдена"));

        var requestUser = group.GroupParticipants.FirstOrDefault(gp => gp.UserId == requestUserId);
        if (requestUser == null)
            return Result.Fail(AppError.Forbidden());

        var user = await usersRepository.Get(userId);
        if(user is null)
            return Result.Fail(AppError.UnprocessableContent("Человек не найден"));

        if (group.GroupParticipants.All(p => p.UserId != userId))
            return Result.Fail(AppError.UnprocessableContent("Участник не состоит в группе"));

        if(requestUser.Role != Role.Organizer)
        {
            if(requestUserId != userId)
                return Result.Fail(AppError.Forbidden());
            
            group.RemoveParticipant(userId);
            await groupsRepository.UnitOfWork.SaveChangesAsync();

            return Result.Ok();
        }

        if (requestUserId == userId) 
            return Result.Fail(AppError.UnprocessableContent("Создатель не может покинуть группу"));

        group.RemoveParticipant(userId);
        await groupsRepository.UnitOfWork.SaveChangesAsync();

        return Result.Ok();
    }
    
}