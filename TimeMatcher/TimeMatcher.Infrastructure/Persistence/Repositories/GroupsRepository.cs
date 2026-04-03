using TimeMatcher.Domain.GroupAggregate;
using Microsoft.EntityFrameworkCore;

namespace TimeMatcher.Infrastructure.Persistence.Repositories;

public class GroupsRepository: IGroupsRepository
{
    private readonly AppDbContext _context;

    public GroupsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Group?> Get(Guid id)
    {
        return await _context.Groups.FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<Group> GetAll()
    {
        return _context.Groups.AsQueryable();
    }

    public async Task<Group> Create(Group group)
    {
        await _context.Groups.AddAsync(group);
        return group;
    }

    public void Delete(Group group)
    {
        _context.Groups.Remove(group);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}