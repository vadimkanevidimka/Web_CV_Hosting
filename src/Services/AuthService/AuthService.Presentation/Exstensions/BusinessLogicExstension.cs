using FluentValidation;
using FluentValidation.AspNetCore;
using AuthService.Buisness.Services.Interfaces;
using AuthService.Buisness.Services.Implementations;
using AuthService.Buisness.Services.Algorithms;
using AuthService.Buisness.Validators;
using AuthService.Buisness.MappingProfiles;
using AuthService.DataAccess.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Presentation.Exstensions;

public static class BusinessLogicExstension
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddServices();
        services.AutoMapperConfigure();
        services.ValidationConfigure();

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
}