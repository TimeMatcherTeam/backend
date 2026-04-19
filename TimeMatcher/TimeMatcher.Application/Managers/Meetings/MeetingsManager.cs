using FluentResults;
using Microsoft.EntityFrameworkCore;
using TimeMatcher.Application.Errors;
using TimeMatcher.Application.Models.Requests.Meeting;
using TimeMatcher.Application.Models.Responses.Meeting;
using TimeMatcher.Domain.AbilityAggregate;
using TimeMatcher.Domain.MeetingAggregate;
using TimeMatcher.Domain.UserAggregate;
using TimeMatcher.Domain.Enums;

namespace TimeMatcher.Application.Managers.Meetings;

internal class MeetingsManager(
    IMeetingsRepository meetingsRepository, 
    IUsersRepository usersRepository, 
    IAbilitiesRepository abilitiesRepository,
    ISlotsRepository slotsRepository
    ): IMeetingsManager
{
    public async Task<Result<MeetingResponse>> GetMeetingById(Guid id, Guid requestUserId)
    {
        var meeting = await meetingsRepository.Get(id);
        if (meeting is null) return Result.Fail(AppError.NotFound());
        if (meeting.MeetingParticipants.All(gp => gp.UserId != requestUserId)) 
            return Result.Fail(AppError.Forbidden());
        var userIds = meeting.MeetingParticipants.Select(gp => gp.UserId);
        var users = await usersRepository.GetUsersByIds(userIds);
        var usersDictionary = users.ToDictionary(u => u.Id);
        return Result.Ok(new MeetingResponse
        {
            Id = meeting.Id,
            Name = meeting.Name,
            Comment = meeting.Comment,
            Link = meeting.Link,
            StartTime = meeting.StartTime,
            EndTime = meeting.EndTime,
            Participants = meeting.MeetingParticipants.Select(gp =>
            {
                var user = usersDictionary[gp.UserId];
                return new MeetingParticipantResponse
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                };
            }).ToArray()
        });
    }

    public async Task<Result<MeetingResponse>> CreateMeeting(CreateMeetingRequest request, Guid requestUserId)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Fail(AppError.UnprocessableContent("Название не может быть пустым"));
        if(request.ParticipantIds.Length <= 1) 
            return Result.Fail(AppError.UnprocessableContent("Необходимо хотя бы 2 участника"));
        if(request.StartTime >= request.EndTime) 
            return Result.Fail(AppError.UnprocessableContent("Начало не может быть позже или равно концу"));
        if(request.StartTime < DateTime.UtcNow) 
            return Result.Fail(AppError.UnprocessableContent("Нельзя чтобы время начала было раньше чем сейчас"));

        var meeting = new Meeting
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Comment = request.Comment,
            Link = null,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            CreatedAt = DateTime.Now
        };
        var users = await usersRepository.GetUsersByIds(request.ParticipantIds);
        if (users.Length != request.ParticipantIds.Length)
            return Result.Fail(AppError.UnprocessableContent("Все участники должны существовать"));
        var userSlots = await slotsRepository.GetFilteredByDateTimeSlotsManyCalendars(
            users.Select(u => u.Calendar.Id).ToArray(), request.StartTime, request.EndTime);
        if (userSlots.Length != 0)
            return Result.Fail(AppError.UnprocessableContent("Не возможно поставить встречу всем участникам, данный промежуток занят у одного из участников"));
        var usersDictionary = users.ToDictionary(u => u.Id);
        var busyAbility =
            await abilitiesRepository.GetAll().FirstOrDefaultAsync(ability => ability.Name.Equals("busy"));
        foreach (var user in users)
        {
            meeting.AddParticipant(user.Id, user.Id == requestUserId ? Role.Organizer : Role.Participant);
            var slot = new Slot
            {
                StartTime = meeting.StartTime,
                EndTime = meeting.EndTime,
                Title = meeting.Name,
                Ability = busyAbility,
                CalendarId = user.Calendar.Id,
                Meeting = meeting
            };
            slotsRepository.Create(slot);
        }

        await meetingsRepository.Create(meeting);
        await meetingsRepository.UnitOfWork.SaveChangesAsync();
        

        return Result.Ok(new MeetingResponse
        {
            Id = meeting.Id,
            Name = meeting.Name,
            Comment = meeting.Comment,
            Link = meeting.Link,
            StartTime = meeting.StartTime,
            EndTime = meeting.EndTime,
            Participants = meeting.MeetingParticipants.Select(gp =>
            {
                var user = usersDictionary[gp.UserId];
                return new MeetingParticipantResponse
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                };
            }).ToArray()
        });
    }

    public async Task<Result<MeetingResponse>> UpdateMeeting(Guid id, UpdateMeetingRequest request, Guid requestUserId)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Fail(AppError.UnprocessableContent("Название не может быть пустым"));
        var meeting = await meetingsRepository.Get(id);
        if (meeting is null) 
            return Result.Fail(AppError.NotFound());
        var requestUser = meeting.MeetingParticipants.FirstOrDefault(gp => gp.UserId == requestUserId);
        if (requestUser is not { Role: Role.Organizer }) 
            return Result.Fail(AppError.Forbidden());

        meeting.Name = request.Name;
        meeting.Comment = request.Comment;
        await meetingsRepository.UnitOfWork.SaveChangesAsync();

        var usersIds = meeting.MeetingParticipants.Select(m => m.UserId);
        var users = await usersRepository.GetUsersByIds(usersIds);
        var usersDictionary = users.ToDictionary(u => u.Id);
        return Result.Ok(new MeetingResponse
        {
            Id = meeting.Id,
            Name = meeting.Name,
            Comment = meeting.Comment,
            Link = meeting.Link,
            StartTime = meeting.StartTime,
            EndTime = meeting.EndTime,
            Participants = meeting.MeetingParticipants.Select(gp =>
            {
                var user = usersDictionary[gp.UserId];
                return new MeetingParticipantResponse
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                };
            }).ToArray()
        });
    }

    public async Task<Result> DeleteMeeting(Guid id, Guid requestUserId)
    {
        var meeting = await meetingsRepository.Get(id);
        if (meeting is null) 
            return Result.Fail(AppError.NotFound());
        var requestUser = meeting.MeetingParticipants.FirstOrDefault(gp => gp.UserId == requestUserId);
        if (requestUser is not { Role: Role.Organizer })
            return Result.Fail(AppError.Forbidden());
        meetingsRepository.Delete(meeting);
        await meetingsRepository.UnitOfWork.SaveChangesAsync();
        return Result.Ok();
    }
}