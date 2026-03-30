namespace TimeMatcher.Domain.AbilityAggregate;

public interface IAbilitiesRepository
{
    public IQueryable<Ability> GetAll();
}