using TimeMatcher.Domain.AbilityAggregate;

namespace TimeMatcher.Infrastructure.Repositories;

internal class AbilitiesRepository(AppDbContext context) : IAbilitiesRepository
{
    public IQueryable<Ability> GetAll()
    {
        return context.Abilities;
    }
}