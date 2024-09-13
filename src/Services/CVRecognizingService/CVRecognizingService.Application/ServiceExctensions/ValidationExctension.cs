using CVRecognizingService.Application.FluentValidation;
using CVRecognizingService.Application.Services.Implementation;
using CVRecognizingService.Application.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CVRecognizingService.Application.ServiceExctensions
{
    public static class ServiceCollectionExctension
    {
        public static IServiceCollection AddValidation (this IServiceCollection services)
        {
            services.AddScoped<IValidator<IFormFile>, FileValidator>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDocumentService, DocumentService>();
            return services;
        }
    }
}
