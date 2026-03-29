using Microsoft.AspNetCore.Mvc;
using TimeMatcher.Application.Responses;

namespace TimeMatcher.Api.Controllers;

[ApiController]
[Route("api/abilities")]
public class AbilitiesController
{
    [HttpGet]
    public async Task<ActionResult<List<AbilityResponse>>> GetAbilities()
    {
        throw new NotImplementedException();
    } 
}