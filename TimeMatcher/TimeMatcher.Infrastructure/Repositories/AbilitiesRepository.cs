using TimeMatcher.Domain;
using TimeMatcher.Domain.AbilityAggregate;

namespace TimeMatcher.Infrastructure.Repositories;

internal class AbilitiesRepository(AppDbContext context) : IAbilitiesRepository
{
    public IQueryable<Ability> GetAll()
    {
        return context.Abilities;
    }

    public async Task<Ability[]> AddRange(Ability[] abilities)
    {
        await context.Abilities.AddRangeAsync(abilities);
        return abilities;
    }

    public IUnitOfWork UnitOfWork => context;
}