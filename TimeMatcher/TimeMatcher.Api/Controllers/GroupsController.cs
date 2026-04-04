using Microsoft.AspNetCore.Mvc;
using TimeMatcher.Application.Models.Requests.Group;
using TimeMatcher.Application.Models.Responses.Group;

namespace TimeMatcher.Api.Controllers;

[ApiController]
[Route("api/groups")]
public class GroupsController : ControllerBase
{
    [HttpGet("{group-id}")]
    public async Task<ActionResult<GroupResponse>> Get([FromRoute(Name="group-id")] Guid groupId)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<ActionResult<GroupResponse>> Create([FromBody] CreateGroupRequest request)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("{group-id}/participants")]
    public async Task<ActionResult<GroupParticipantResponse>> AddParticipant([FromBody] Guid userId, [FromRoute(Name = "group-id")] Guid groupId)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{group-id}")]
    public async Task<ActionResult<GroupResponse>> Update([FromRoute(Name="group-id")] Guid groupId,
        [FromBody] UpdateGroupRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{group-id}")]
    public async Task<ActionResult> Delete([FromRoute(Name = "group-id")] Guid groupId)
    {
        throw new NotImplementedException();
    }
    

    [HttpDelete("{group-id}/participants/{user-id}")]
    public async Task<ActionResult> DeleteParticipant([FromRoute(Name = "user-id")] Guid userId, [FromRoute(Name = "group-id")] Guid groupId)
    {
        throw new NotImplementedException();
    }
}