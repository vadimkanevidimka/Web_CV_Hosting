using System.Text;
using AuthService.DataAccess.Entities;
using AuthService.DataAccess.Persistans;
using AuthService.DataAccess.Persistans.DbContext;
using AuthService.Presentation.Middlewares;
using AuthService.Presentation.Policies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Presentation.Exstensions;

public static class PresentationExstension
{
    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddControllers();
        services.AuthConfigure(configuration);
        services.AddSwagerWithAuth();
        services.AddScoped<ExceptionHandlingMiddleware>();

        return services;
    }

    public static WebApplication StartApplication(this WebApplication webApplication)
    {
        webApplication.DbInitialize();
        webApplication.UseAuthentication();
        webApplication.UseAuthorization();
        webApplication.MapControllers();
        webApplication.SwaggerStart();
        webApplication.SeedData();
        webApplication.UseMiddleware<ExceptionHandlingMiddleware>();
        webApplication.Run();

        return webApplication;
    }

    private static WebApplication DbInitialize(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        try
        {
            var appDbContext = scope.ServiceProvider.GetRequiredService<AuthorizationDbContext>();
            DbInitializer.Initialize(appDbContext);
        }
        catch (Exception ex)
        {
        }

        return webApplication;
    }

    private static IServiceCollection AuthConfigure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = configuration["JwtSettings:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["JwtSettings:AccessTokenSecret"]!)),
                ValidateActor = true,
                ValidateIssuer = true,
                ValidateAudience = false,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true
            };
        });
        services.AddAuthorization(option =>
        {
            option.AddPolicy(Policies.Policies.RequireStaff,
                policy => policy.Requirements.Add(new RolesRequirement([Roles.Admin])));
        });

        return services;
    }

    private static WebApplication SwaggerStart(this WebApplication webApplication)
    {
        if (webApplication.Environment.IsDevelopment())
        {
            webApplication.UseSwagger();
            webApplication.UseSwaggerUI();
            webApplication.ApplyMigrations();
        }

        return webApplication;
    }

    private static WebApplication SeedData(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        DbInitializer.Seed(userManager, roleManager);

        return webApplication;
    }
}