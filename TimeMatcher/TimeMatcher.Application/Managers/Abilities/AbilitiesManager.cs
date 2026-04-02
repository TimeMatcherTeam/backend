using TimeMatcher.Application.Responses;
using TimeMatcher.Domain.AbilityAggregate;

namespace TimeMatcher.Application.Managers.Abilities;

public class AbilitiesManager(IAbilitiesRepository abilitiesRepository): IAbilitiesManager
{
    public async Task<AbilityResponse[]> GetAllAbilities()
    {
        return abilitiesRepository.GetAll().Select(ability => new AbilityResponse
        {
            Id = ability.Id,
            Ability = ability.Name
        }).ToArray();
    }
}