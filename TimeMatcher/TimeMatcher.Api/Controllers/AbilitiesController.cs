using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeMatcher.Application.Managers.Abilities;
using TimeMatcher.Application.Models.Responses;

namespace TimeMatcher.Api.Controllers;

[ApiController]
[Route("api/abilities")]
public class AbilitiesController(IAbilitiesManager abilitiesManager) : ControllerBase
{
    /// <summary>
    /// Получение всех типов занятости
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AbilityResponse[]>> GetAllAbilities()
    {
        var abilities = await abilitiesManager.GetAllAbilities();
        return Ok(abilities);
    } 
}