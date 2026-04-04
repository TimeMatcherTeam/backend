using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeMatcher.Domain.AbilityAggregate;
using TimeMatcher.Domain.GroupAggregate;
using TimeMatcher.Domain.MeetingAggregate;
using TimeMatcher.Domain.UserAggregate;

namespace TimeMatcher.Infrastructure;

public class AppDbContext : IdentityDbContext<User, UserRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<Ability> Abilities => Set<Ability>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("time-matcher");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}