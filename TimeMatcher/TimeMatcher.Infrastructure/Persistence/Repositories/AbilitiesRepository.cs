using TimeMatcher.Domain.AbilityAggregate;

namespace TimeMatcher.Infrastructure.Persistence.Repositories;

public class AbilitiesRepository: IAbilitiesRepository
{
    public async Task<IQueryable<Ability>> Get()
    {
        throw new NotImplementedException();
    }
}