using Microsoft.Extensions.DependencyInjection;
using TimeMatcher.Application.Managers.Abilities;
using TimeMatcher.Application.Managers.Groups;
using TimeMatcher.Application.Managers.Meetings;
using TimeMatcher.Application.Managers.Users;

namespace TimeMatcher.Application;

public static class BuisnessLogicConfiguration
{
    public static IServiceCollection AddBuisnessLogic(this IServiceCollection services)
    {
        return services
            .AddScoped<IUsersManager, UsersManager>()
            .AddScoped<IGroupsManager, GroupsManager>()
            .AddScoped<IMeetingsManager, MeetingsManager>()
            .AddScoped<IAbilitiesManager, AbilitiesManager>()
            .AddScoped<IAccessTokenGenerator, AccessTokenGenerator>();
    }
}