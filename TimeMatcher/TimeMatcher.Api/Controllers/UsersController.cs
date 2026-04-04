using Microsoft.AspNetCore.Mvc;
using TimeMatcher.Api.Auth;
using TimeMatcher.Api.Errors;
using TimeMatcher.Application.Managers.Users;
using TimeMatcher.Application.Models.Requests.User;
using TimeMatcher.Application.Models.Responses.Group;
using TimeMatcher.Application.Models.Responses.Meeting;
using TimeMatcher.Application.Models.Responses.User;

namespace TimeMatcher.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUsersManager usersManager) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<UserResponse[]>> GetUsers([FromQuery] GetUsersRequest request)
    {
        var result = await usersManager.GetUsers(request);
        return result.ToActionResult();
    }
    
    [HttpGet("{user-id}/groups")]
    public async Task<ActionResult<GroupResponse[]>> GetGroups([FromRoute(Name="user-id")] Guid userId, [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.GetUserGroups(userId, requestedUserId);
        return result.ToActionResult();
    }
    
    [HttpGet("{user-id}/meetings")]
    public async Task<ActionResult<MeetingResponse[]>> GetMeetings([FromRoute(Name="user-id")] Guid userId, [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.GetUserMeetings(userId, requestedUserId);
        return result.ToActionResult();
    }
    
    [HttpGet("{user-id}/calendar")]
    public async Task<ActionResult<CalendarResponse>> GetCalendar([FromRoute(Name="user-id")] Guid userId, [FromQuery] RequestedPeriod period, [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.GetUserCalendar(userId, period, requestedUserId);
        return result.ToActionResult();
    }
    
    [HttpGet("{user-id}")]
    public async Task<ActionResult<UserResponse>> GetUser([FromRoute(Name="user-id")] Guid userId, [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.GetUserById(userId, requestedUserId);
        return result.ToActionResult();
    }
    
    [HttpPost("/merge-calendar")]
    public async Task<ActionResult<CalendarResponse>> GetMergedCalendar([FromBody] GetMergedCalendarRequest request, [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.GetMergedCalendar(request, requestedUserId);
        return result.ToActionResult();
    }
    
    [HttpPost("{user-id}/calendar/slots")]
    public async Task<ActionResult<SlotResponse>> AddSlot([FromBody] SlotRequest request, [FromRoute(Name = "user-id")] Guid userId, [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.CreateSlot(request, userId, requestedUserId);
        return result.ToActionResult();
    }
    
    [HttpPut("{user-id}/calendar/slots/{slot-id}")]
    public async Task<ActionResult<SlotResponse>> UpdateSlot([FromRoute(Name="slot-id")] Guid slotId, [FromBody] SlotRequest request, [FromRoute(Name = "user-id")] Guid userId, [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.UpdateSlot(slotId, userId, request, requestedUserId);
        return result.ToActionResult();
    }

    [HttpPut("{user-id}")]
    public async Task<ActionResult<UserResponse>> UpdateUser([FromRoute(Name="user-id")] Guid userId,
        [FromBody] UpdateUserRequest request, [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.UpdateUser(userId, request, requestedUserId);
        return result.ToActionResult();
    }

    [HttpDelete("{user-id}")]
    public async Task<ActionResult> DeleteUser([FromRoute(Name = "user-id")] Guid userId, [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.DeleteUser(userId, requestedUserId);
        return result.ToActionResult();
    }
    
    [HttpDelete("{user-id}/calendar/slots/{slot-id}")]
    public async Task<ActionResult> DeleteSlot([FromRoute(Name = "slot-id")] Guid slotId, [FromRoute(Name = "user-id")] Guid userId, [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.DeleteSlot(slotId, userId, requestedUserId);
        return result.ToActionResult();
    }
    
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    [HttpPost]
    [ProducesResponseType<string>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginInfoResponse>> Register([FromBody] RegisterUserRequest request)
    {
        var result = await usersManager.RegisterAsync(request);
        return result.ToActionResult();
    }

    /// <summary>
    /// Аутентификация пользователя и получение JWT-токена
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginInfoResponse>> Login([FromBody] LoginUserRequest request)
    {
        var result = await usersManager.LoginAsync(request.Email, request.Password);
        return result.ToActionResult();
    }

    [HttpPut("{user-id}/change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginInfoResponse>> ChangePassword([FromRoute(Name = "user-id")] Guid userId, [FromBody] string oldPassword, [FromBody] string newPassword, [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.ChangePassword(oldPassword, newPassword, requestedUserId);
        return result.ToActionResult();
    }
}