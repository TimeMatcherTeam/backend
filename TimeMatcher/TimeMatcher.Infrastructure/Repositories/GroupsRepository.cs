using Microsoft.EntityFrameworkCore;
using TimeMatcher.Domain.GroupAggregate;

namespace TimeMatcher.Infrastructure.Repositories;

internal class GroupsRepository(AppDbContext context) : IGroupsRepository
{
    public async Task<Group?> Get(Guid id)
    {
        return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
    }

    public IQueryable<Group> GetAll()
    {
        return context.Groups.Include(g => g.GroupParticipants);
    }

    public async Task<Group> Create(Group group)
    {
        await context.Groups.AddAsync(group);
        return group;
    }

    public void Delete(Group group)
    {
        context.Groups.Remove(group);
    }

    public async Task SaveChanges()
    {
        await context.SaveChangesAsync();
    }
}