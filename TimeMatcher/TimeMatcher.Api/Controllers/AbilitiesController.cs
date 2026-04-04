using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeMatcher.Application.Models.Responses;

namespace TimeMatcher.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/abilities")]
public class AbilitiesController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<AbilityResponse[]>> GetAllAbilities()
    {
        throw new NotImplementedException();
    } 
}