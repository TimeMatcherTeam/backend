using Microsoft.EntityFrameworkCore;
using TimeMatcher.Application.Models.Requests;
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

    public async Task<AbilityResponse[]> AddAbilities(AbilityRequest[] abilityRequests)
    {
        var abilities = abilityRequests.Select(request => new Ability { Name = request.Name }).ToArray();
        var addedAbilities = await abilitiesRepository.AddRange(abilities);
        await abilitiesRepository.UnitOfWork.SaveChangesAsync();
        return addedAbilities.Select(a => new AbilityResponse
        {
            Id = a.Id,
            Ability = a.Name
        }).ToArray();
    }
}