using TimeMatcher.Application.Managers.Abilities;
using TimeMatcher.Application.Models.Requests;

namespace TimeMatcher.Api;

public class AbilitiesCreator
{
    public static async Task CreateAbilities(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var manager = scope.ServiceProvider.GetRequiredService<IAbilitiesManager>();

        var abilities = await manager.GetAllAbilities();
        if (abilities.Length == 0)
        {
            await manager.AddAbilities(
            [
                new AbilityRequest { Name = "busy" }, 
                new AbilityRequest { Name = "partially_busy" }
            ]);
        }
    }
}