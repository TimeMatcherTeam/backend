namespace TimeMatcher.Domain;

public interface IRepository
{
    public IUnitOfWork UnitOfWork { get; }
}