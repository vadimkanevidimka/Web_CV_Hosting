using AuthService.DataAccess.Entities;
using AuthService.DataAccess.Persistans;
using AuthService.DataAccess.Persistans.DbContext;
using AuthService.DataAccess.Persistans.Repositories.Implementations;
using AuthService.DataAccess.Persistans.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AuthService.Presentation.Exstensions;

public static class DataAccessExstension
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.IdentityConfigure();
        services.DbConfigure(configuration);
        services.RepositoriesConfigure();

        return services;
    }

    private static IServiceCollection DbConfigure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthorizationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("Postgres")));
        return services;
    }

    private static IServiceCollection RepositoriesConfigure(this IServiceCollection services)
    {
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }

    private static IServiceCollection IdentityConfigure(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AuthorizationDbContext>()
            .AddDefaultTokenProviders();
        return services;
    }
}