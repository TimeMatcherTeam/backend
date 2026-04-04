using TimeMatcher.Application.Models.Responses;

namespace TimeMatcher.Application.Managers.Abilities;

public interface IAbilitiesManager
{
    Task<AbilityResponse[]> GetAllAbilities();
}