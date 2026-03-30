using Microsoft.AspNetCore.Mvc;
using TimeMatcher.Application.Responses;

namespace TimeMatcher.Api.Controllers;

[ApiController]
[Route("api/abilities")]
public class AbilitiesController
{
    [HttpGet]
    public async Task<ActionResult<AbilityResponse[]>> GetAllAbilities()
    {
        throw new NotImplementedException();
    } 
}