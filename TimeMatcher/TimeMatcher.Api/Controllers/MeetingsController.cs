using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeMatcher.Api.Auth;
using TimeMatcher.Api.Errors;
using TimeMatcher.Application.Managers.Meetings;
using TimeMatcher.Application.Models.Requests.Meeting;
using TimeMatcher.Application.Models.Responses.Meeting;

namespace TimeMatcher.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/meetings")]
public class MeetingsController(IMeetingsManager meetingsManager) : ControllerBase
{
    
    /// <summary>
    /// Получить встречу
    /// </summary>
    [HttpGet("{meeting-id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<MeetingResponse>> Get(
        [FromRoute(Name="meeting-id")] Guid meetingId, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await meetingsManager.GetMeetingById(meetingId, requestedUserId);
        return result.ToActionResult();
    }

    /// <summary>
    /// Создать встречу
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<MeetingResponse>> Create(
        [FromBody] CreateMeetingRequest request, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await meetingsManager.CreateMeeting(request, requestedUserId);
        return result.ToActionResult();
    }

    /// <summary>
    /// Обновить данные встречи
    /// </summary>
    [HttpPut("{meeting-id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<MeetingResponse>> Update(
        [FromRoute(Name="meeting-id")] Guid meetingId,
        [FromBody] UpdateMeetingRequest request, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await meetingsManager.UpdateMeeting(meetingId, request, requestedUserId);
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить встречу
    /// </summary>
    [HttpDelete("{meeting-id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Delete(
        [FromRoute(Name = "meeting-id")] Guid meetingId, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await meetingsManager.DeleteMeeting(meetingId, requestedUserId);
        return result.ToActionResult();
    }
}