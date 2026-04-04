using Microsoft.AspNetCore.Identity;
using TimeMatcher.Domain.Enums;
using TimeMatcher.Domain.UserAggregate;

namespace TimeMatcher.Api.Auth;

public class RoleCreator
{
    public static async Task CreateRolesInSystemAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UserRole>>();

        foreach (var role in Enum.GetValues<ServiceRole>())
        {
            var roleName = $"{role}";
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new UserRole(roleName));
        }
    }
}