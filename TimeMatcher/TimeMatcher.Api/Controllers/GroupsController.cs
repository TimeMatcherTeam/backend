using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeMatcher.Api.Auth;
using TimeMatcher.Api.Errors;
using TimeMatcher.Application.Managers.Groups;
using TimeMatcher.Application.Models.Requests.Group;
using TimeMatcher.Application.Models.Responses.Group;

namespace TimeMatcher.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/groups")]
public class GroupsController(IGroupsManager groupsManager) : ControllerBase
{
    /// <summary>
    /// Получить группу
    /// </summary>
    [HttpGet("{group-id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<GroupResponse>> Get(
        [FromRoute(Name="group-id")] Guid groupId,
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await groupsManager.GetGroupById(groupId, requestedUserId);
        return result.ToActionResult();
    }

    /// <summary>
    /// Создать группу
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<GroupResponse>> Create(
        [FromBody] CreateGroupRequest request, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await groupsManager.CreateGroup(request, requestedUserId);
        return result.ToActionResult();
    }
    
    /// <summary>
    /// Добавить участника в группу
    /// </summary>
    [HttpPost("{group-id}/participants")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<GroupParticipantResponse>> AddParticipant(
        [FromBody] Guid userId, 
        [FromRoute(Name = "group-id")] Guid groupId, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await groupsManager.AddParticipant(groupId, userId, requestedUserId);
        return result.ToActionResult();
    }

    /// <summary>
    /// Обновить данные группы
    /// </summary>
    [HttpPut("{group-id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<GroupResponse>> Update(
        [FromRoute(Name="group-id")] Guid groupId,
        [FromBody] UpdateGroupRequest request, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await groupsManager.UpdateGroup(groupId, request, requestedUserId);
        return result.ToActionResult();
    }

    /// <summary>
    /// Удалить группу
    /// </summary>
    [HttpDelete("{group-id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> Delete(
        [FromRoute(Name = "group-id")] Guid groupId, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await groupsManager.DeleteGroup(groupId, requestedUserId);
        return result.ToActionResult();
    }
    
    /// <summary>
    /// Удалить участника группы(покинуть группу)
    /// </summary>
    [HttpDelete("{group-id}/participants/{user-id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> DeleteParticipant(
        [FromRoute(Name = "user-id")] Guid userId, 
        [FromRoute(Name = "group-id")] Guid groupId, 
        [FromServices] IIdentityService identityService)
    {
        var requestedUserId = identityService.GetUserIdentity();
        var result = await groupsManager.DeleteParticipant(groupId, userId, requestedUserId);
        return result.ToActionResult();
    }
}