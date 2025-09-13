using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CVRecognizingService.Application.ServiceExctensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerWithAuth(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "CV Recognizing Service API", 
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Support",
                        Email = "support@cvservice.com"
                    }
                });

                // JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                };

                c.AddSecurityDefinition(securityScheme.Name, securityScheme);

                // Важно для .NET 8
                c.UseAllOfToExtendReferenceSchemas();
                c.UseOneOfForPolymorphism();
            });

            return services;
        }
    }
}