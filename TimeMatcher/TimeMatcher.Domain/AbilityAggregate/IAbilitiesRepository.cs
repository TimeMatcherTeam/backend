namespace TimeMatcher.Domain.AbilityAggregate;

public interface IAbilitiesRepository: IRepository
{
    public IQueryable<Ability> GetAll();
    
    public Task<Ability[]> AddRange(Ability[] abilities);
}