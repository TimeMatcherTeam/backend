using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using TimeMatcher.Api;
using TimeMatcher.Api.Auth;
using TimeMatcher.Api.Extensions;
using TimeMatcher.Application;
using TimeMatcher.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddHttpContextAccessor()
    .AddIdentityServices(builder.Configuration)
    .AddScoped<IIdentityService, IdentityService>()
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddBuisnessLogic()
    .AddSwagger();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

await RoleCreator.CreateRolesInSystemAsync(app);


app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();