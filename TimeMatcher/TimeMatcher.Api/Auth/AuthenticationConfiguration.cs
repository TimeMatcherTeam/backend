using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TimeMatcher.Application.Managers.Users;
using TimeMatcher.Domain.UserAggregate;
using TimeMatcher.Infrastructure;

namespace TimeMatcher.Api.Auth;

public static class AuthenticationConfiguration
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        var authOptions = new AuthOptions();
        configuration.Bind(AuthOptions.SectionName, authOptions);
        services.AddSingleton(authOptions);
        
        services
            .AddIdentity<User, UserRole>(options =>
            {
                options.User.RequireUniqueEmail = true; 
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOptions => ConfigureJwtOptions(jwtOptions, authOptions));

        return services;
    }
    
    private static void ConfigureJwtOptions(JwtBearerOptions options, AuthOptions authOptions)
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = authOptions.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
            RoleClaimType = ClaimTypes.Role
        };
    }
}