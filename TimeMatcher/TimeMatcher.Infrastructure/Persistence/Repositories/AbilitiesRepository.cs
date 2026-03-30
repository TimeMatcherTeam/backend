using TimeMatcher.Domain.AbilityAggregate;

namespace TimeMatcher.Infrastructure.Persistence.Repositories;

public class AbilitiesRepository: IAbilitiesRepository
{
    public IQueryable<Ability> GetAll()
    {
        throw new NotImplementedException();
    }
}