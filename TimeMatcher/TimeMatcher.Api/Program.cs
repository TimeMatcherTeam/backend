using System.Text.Json;
using System.Text.Json.Serialization;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",
                "https://timematcher.ru",
                "http://timematcher.ru"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails(); 
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

await RoleCreator.CreateRolesInSystemAsync(app);
await AbilitiesCreator.CreateAbilities(app);


app.UseExceptionHandler();
app.UseSwagger();


app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();