using Microsoft.EntityFrameworkCore;
using TimeMatcher.Application.Models.Responses;
using TimeMatcher.Domain.AbilityAggregate;

namespace TimeMatcher.Application.Managers.Abilities;

internal class AbilitiesManager(IAbilitiesRepository abilitiesRepository): IAbilitiesManager
{
    public async Task<AbilityResponse[]> GetAllAbilities()
    {
        return await abilitiesRepository.GetAll().Select(ability => new AbilityResponse
        {
            Id = ability.Id,
            Ability = ability.Name
        }).ToArrayAsync();
    }
}