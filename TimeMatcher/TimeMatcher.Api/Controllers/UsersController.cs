using Microsoft.AspNetCore.Authorization;
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
    /// <summary>
    /// Получить пользователей, подходящих под параметры запроса
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<UserResponse[]>> GetUsers(
        [FromQuery] GetUsersRequest request)
    {
        var result = await usersManager.GetUsers(request);
        return result.ToActionResult();
    }
    
    /// <summary>
    /// Получить группы пользователя
    /// </summary>
    [HttpGet("{user-id}/groups")]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<GroupResponse[]>> GetGroups(
        [FromRoute(Name="user-id")] Guid userId, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.GetUserGroups(userId, requestedUserId);
        return result.ToActionResult();
    }
    
    /// <summary>
    /// Получить встречи пользователя
    /// </summary>
    [HttpGet("{user-id}/meetings")]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<MeetingResponse[]>> GetMeetings(
        [FromRoute(Name="user-id")] Guid userId, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.GetUserMeetings(userId, requestedUserId);
        return result.ToActionResult();
    }
    
    /// <summary>
    /// Получить календарь пользователя в заданный период
    /// </summary>
    [HttpGet("{user-id}/calendar")]
    [ProducesResponseType<string>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [Authorize]
    public async Task<ActionResult<CalendarResponse>> GetCalendar(
        [FromRoute(Name="user-id")] Guid userId, 
        [FromQuery] RequestedPeriod period, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.GetUserCalendar(userId, period, requestedUserId);
        return result.ToActionResult();
    }
    
    /// <summary>
    /// Получить пользователя
    /// </summary>
    [HttpGet("{user-id}")]
    [ProducesResponseType<string>(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<UserResponse>> GetUser(
        [FromRoute(Name="user-id")] Guid userId, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.GetUserById(userId, requestedUserId);
        return result.ToActionResult();
    }
    
    /// <summary>
    /// Получить совместный календарь пользователей
    /// </summary>
    [HttpPost("merge-calendar")]
    [ProducesResponseType<string>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [Authorize]
    public async Task<ActionResult<MergedCalendarResponse>> GetMergedCalendar(
        [FromBody] GetMergedCalendarRequest request, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.GetMergedCalendar(request, requestedUserId);
        return result.ToActionResult();
    }
    
    /// <summary>
    /// Добавить слот(мероприятие)
    /// </summary>
    [HttpPost("{user-id}/calendar/slots")]
    [ProducesResponseType<string>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [Authorize]
    public async Task<ActionResult<SlotResponse>> AddSlot(
        [FromBody] SlotRequest request, 
        [FromRoute(Name = "user-id")] Guid userId, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.CreateSlot(request, userId, requestedUserId);
        return result.ToActionResult();
    }
    
    /// <summary>
    /// Изменить слот(мероприятие)
    /// </summary>
    [HttpPut("{user-id}/calendar/slots/{slot-id}")]
    [ProducesResponseType<string>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult<SlotResponse>> UpdateSlot(
        [FromRoute(Name="slot-id")] Guid slotId, 
        [FromBody] SlotRequest request, 
        [FromRoute(Name = "user-id")] Guid userId, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.UpdateSlot(slotId, userId, request, requestedUserId);
        return result.ToActionResult();
    }

    /// <summary>
    /// Изменить данные пользователя
    /// </summary>
    [HttpPut("{user-id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize]
    public async Task<ActionResult<UserResponse>> UpdateUser(
        [FromRoute(Name="user-id")] Guid userId,
        [FromBody] UpdateUserRequest request, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.UpdateUser(userId, request, requestedUserId);
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить пользователя
    /// </summary>
    [HttpDelete("{user-id}")]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult> DeleteUser(
        [FromRoute(Name = "user-id")] Guid userId, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.DeleteUser(userId, requestedUserId);
        return result.ToActionResult();
    }
    
    /// <summary>
    /// Удалить слот(мероприятие)
    /// </summary>
    [HttpDelete("{user-id}/calendar/slots/{slot-id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<string>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<ActionResult> DeleteSlot(
        [FromRoute(Name = "slot-id")] Guid slotId, 
        [FromRoute(Name = "user-id")] Guid userId, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.DeleteSlot(slotId, userId, requestedUserId);
        return result.ToActionResult();
    }
    
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    [HttpPost]
    [ProducesResponseType<string>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginInfoResponse>> Register(
        [FromBody] RegisterUserRequest request)
    {
        var result = await usersManager.RegisterAsync(request);
        return result.ToActionResult();
    }

    /// <summary>
    /// Аутентификация пользователя и получение JWT-токена
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<LoginInfoResponse>> Login(
        [FromBody] LoginUserRequest request)
    {
        var result = await usersManager.LoginAsync(request.Email, request.Password);
        return result.ToActionResult();
    }

    /// <summary>
    /// Сменить пароль
    /// </summary>
    [HttpPut("{user-id}/change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [Authorize]
    public async Task<ActionResult<LoginInfoResponse>> ChangePassword(
        [FromRoute(Name = "user-id")] Guid userId, 
        [FromBody] ChangePasswordRequest request, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await usersManager.ChangePassword(userId,request, requestedUserId);
        return result.ToActionResult();
    }
}