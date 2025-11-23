using AuthService.DataAccess.Entities;
using AuthService.DataAccess.Persistans;
using AuthService.DataAccess.Persistans.DbContext;
using AuthService.Presentation.Middlewares;
using AuthService.Presentation.Policies;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthService.Presentation.Exstensions;

public static class PresentationExstension
{
    private const string CorsPolicyName = "DefaultCors";

    public static IServiceCollection AddPresentationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEndpointsApiExplorer();
        services.AddControllers();
        services.AuthConfigure(configuration);
        services.AddSwagerWithAuth();
        services.AddScoped<ExceptionHandlingMiddleware>();

        // Configure CORS policy from configuration (e.g. Cors:AllowedOrigins)
        var origins = configuration["Cors:AllowedOrigins"]?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? new[] { "http://localhost:5173" };
        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, builder =>
            {
                builder.WithOrigins(origins)
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials();
            });
        });

        return services;
    }

    public static WebApplication StartApplication(this WebApplication webApplication)
    {
        webApplication.DbInitialize();

        // Ensure routing is configured before CORS/auth
        webApplication.UseRouting();

        // Apply CORS policy early so preflight OPTIONS are handled before authentication
        webApplication.UseCors(CorsPolicyName);

        // Exception handling middleware should be early to catch errors
        webApplication.UseMiddleware<ExceptionHandlingMiddleware>();

        webApplication.UseAuthentication();
        webApplication.UseAuthorization();

        webApplication.MapControllers();

        webApplication.SwaggerStart();
        webApplication.SeedData();

        webApplication.UseHangfireDashboard("/dashboard",
            new DashboardOptions()
            {
                Authorization = new[]{
                    new LocalRequestsOnlyAuthorizationFilter()
                }
            });

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

            options.Events = new JwtBearerEvents()
            {
                OnMessageReceived = c =>
                {
                    // Prefer cookie token, fallback to Authorization header
                    if (c.Request.Cookies.TryGetValue("key", out var cookieToken) && !string.IsNullOrWhiteSpace(cookieToken))
                    {
                        c.Token = cookieToken;
                    }
                    else if (c.Request.Headers.TryGetValue("Authorization", out var authHeader))
                    {
                        var header = authHeader.ToString();
                        if (header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        {
                            c.Token = header.Substring("Bearer ".Length).Trim();
                        }
                    }

                    return Task.CompletedTask;
                }
            };
        });
        services.AddAuthorization(option =>
        {
            option.AddPolicy(Policies.Policies.RequireStaff,
                policy => policy.Requirements.Add(new RolesRequirement([Roles.Admin, Roles.User])));
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