using Microsoft.EntityFrameworkCore;
using TimeMatcher.Domain.AbilityAggregate;
using TimeMatcher.Domain.UserAggregate;
using TimeMatcher.Domain.GroupAggregate;
using TimeMatcher.Domain.MeetingAggregate;

namespace TimeMatcher.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<Ability> Abilities => Set<Ability>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}