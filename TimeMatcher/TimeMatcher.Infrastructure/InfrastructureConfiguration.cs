using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TimeMatcher.Domain.AbilityAggregate;
using TimeMatcher.Domain.GroupAggregate;
using TimeMatcher.Domain.MeetingAggregate;
using TimeMatcher.Domain.UserAggregate;
using TimeMatcher.Infrastructure.Repositories;

namespace TimeMatcher.Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager manager)
    {
        return services.AddScoped<IUsersRepository, UsersRepository>()
            .AddScoped<IGroupsRepository, GroupsRepository>()
            .AddScoped<IMeetingsRepository, MeetingsRepository>()
            .AddScoped<IAbilitiesRepository, AbilitiesRepository>()
            .AddDbContext<AppDbContext>(options => options.UseNpgsql(manager.GetConnectionString("DefaultConnection")));
    }
}