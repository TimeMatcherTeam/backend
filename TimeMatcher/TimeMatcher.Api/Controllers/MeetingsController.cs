using Microsoft.AspNetCore.Mvc;
using TimeMatcher.Application.Models.Requests.Meeting;
using TimeMatcher.Application.Models.Responses.Meeting;

namespace TimeMatcher.Api.Controllers;

[ApiController]
[Route("api/meetings")]
public class MeetingsController : ControllerBase
{
    
    [HttpGet("{meeting-id}")]
    public async Task<ActionResult<MeetingResponse>> Get([FromRoute(Name="meeting-id")] Guid meetingId)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<ActionResult<MeetingResponse>> Create([FromBody] CreateMeetingRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{meeting-id}")]
    public async Task<ActionResult<MeetingResponse>> Update([FromRoute(Name="meeting-id")] Guid meetingId,
        [FromBody] UpdateMeetingRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{meeting-id}")]
    public async Task<ActionResult> Delete([FromRoute(Name = "meeting-id")] Guid meetingId)
    {
        throw new NotImplementedException();
    }
}