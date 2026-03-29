using TimeMatcher.Application.Responses;

namespace TimeMatcher.Application.Managers.Abilities;

public interface IAbilitiesManager
{
    Task<AbilityResponse[]> GetAllAbilities();
}