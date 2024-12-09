using FluentValidation;
using FluentValidation.AspNetCore;
using AuthService.Buisness.Services.Interfaces;
using AuthService.Buisness.Services.Implementations;
using AuthService.Buisness.Services.Algorithms;
using AuthService.Buisness.Validators;
using AuthService.Buisness.MappingProfiles;
using Hangfire;
using Hangfire.PostgreSql;

namespace AuthService.Presentation.Exstensions;

public static class BusinessLogicExstension
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddServices();
        services.AutoMapperConfigure();
        services.ValidationConfigure();
        services.AddHangfireService(configuration);

        return services;
    }

    private static IServiceCollection ValidationConfigure(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(SimpleValidators).Assembly);
        services.AddFluentValidationAutoValidation();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<TokensGenerator>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IBackgroundRefreshTokenService, BackgroundRefreshTokenService>();
        return services;
    }

    private static IServiceCollection AutoMapperConfigure(this IServiceCollection services)
    {
        services.AddAutoMapper(new[]
        {
            typeof(TokenMappingProfile),
            typeof(UserMappingProfile)
        });

        return services;
    }

    private static IServiceCollection AddHangfireService(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHangfire(options =>
        {
            options.UseSimpleAssemblyNameTypeSerializer()
                .UsePostgreSqlStorage(options =>
                    options.UseNpgsqlConnection(configuration.GetConnectionString("HangFireDatabase")));
        });
        services.AddHangfireServer();
        services.AddScoped<IBackgroundRefreshTokenService, BackgroundRefreshTokenService>();

        return services;
    }
}