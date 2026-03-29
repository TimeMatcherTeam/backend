namespace TimeMatcher.Domain.AbilityAggregate;

public interface IAbilitiesRepository
{
    public Task<IQueryable<Ability>> Get();
}