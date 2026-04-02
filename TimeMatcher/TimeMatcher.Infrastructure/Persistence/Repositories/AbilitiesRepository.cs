using TimeMatcher.Domain.AbilityAggregate;

namespace TimeMatcher.Infrastructure.Persistence.Repositories;

public class AbilitiesRepository: IAbilitiesRepository
{
    private readonly AppDbContext _context;

    public AbilitiesRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<Ability> GetAll()
    {
        return _context.Abilities.AsQueryable();
    }
}