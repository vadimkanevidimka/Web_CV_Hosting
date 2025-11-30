using Microsoft.Extensions.DependencyInjection;
using ProfileService.Domain.ApplicantProfile;
using ProfileService.Infastructure.Repositories;

namespace ProfileService.Application.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ProfileRepository>();
            return services;
        }
    }
}
