﻿using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using CVRecognizingService.Domain.DTOs.Outgoing;
using CVRecognizingService.Application.FluentValidation;

namespace CVRecognizingService.Application.ServiceExctensions;

public static class ServiceCollectionExctension
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<IFormFile>, FileValidator>();
        services.AddScoped<IValidator<DocumentDto>, DocumentDtoValidator>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services;
    }
}