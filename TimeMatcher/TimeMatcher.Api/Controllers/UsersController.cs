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
    public async Task<ActionResult<UserResponse[]>> GetUsers([FromQuery] GetUsersRequest request, [FromServices] IIdentityService identityService)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("{user-id}/groups")]
    public async Task<ActionResult<GroupResponse[]>> GetGroups([FromRoute(Name="user-id")] Guid userId)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("{user-id}/meetings")]
    public async Task<ActionResult<MeetingResponse[]>> GetMeetings([FromRoute(Name="user-id")] Guid userId)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("{user-id}/calendar")]
    public async Task<ActionResult<CalendarResponse>> GetCalendar([FromRoute(Name="user-id")] Guid userId, [FromQuery] RequestedPeriod period)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("{user-id}")]
    public async Task<ActionResult<UserResponse>> GetUser([FromRoute(Name="user-id")] Guid userId)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("/merge-calendar")]
    public async Task<ActionResult<CalendarResponse>> GetMergedCalendar([FromBody] GetMergedCalendarRequest request)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("{user-id}/calendar/slots")]
    public async Task<ActionResult<SlotResponse>> AddSlot([FromBody] SlotRequest request, [FromRoute(Name = "user-id")] Guid userId)
    {
        throw new NotImplementedException();
    }
    
    [HttpPut("{user-id}/calendar/slots/{slot-id}")]
    public async Task<ActionResult<SlotResponse>> UpdateSlot([FromRoute(Name="slot-id")] Guid slotId, [FromBody] SlotRequest request, [FromRoute(Name = "user-id")] Guid userId)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{user-id}")]
    public async Task<ActionResult<UserResponse>> UpdateUser([FromRoute(Name="user-id")] Guid userId,
        [FromBody] UpdateUserRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{user-id}")]
    public async Task<ActionResult> DeleteUser([FromRoute(Name = "user-id")] Guid userId)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("{user-id}/calendar/slots/{slot-id}")]
    public async Task<ActionResult> DeleteSlot([FromRoute(Name = "slot-id")] Guid slotId, [FromRoute(Name = "user-id")] Guid userId)
    {
        throw new NotImplementedException();
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
    [HttpPost("me/login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginInfoResponse>> Login([FromBody] LoginUserRequest request)
    {
        var result = await usersManager.LoginAsync(request.Email, request.Password);
        return result.ToActionResult();
    }
}