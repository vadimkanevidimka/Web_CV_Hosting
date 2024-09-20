using CVRecognizingService.Infrastructure.DataAccess.DBContext;
using Microsoft.Extensions.DependencyInjection;

namespace CVRecognizingService.Application.ServiceExctensions
{
    public static class DBContextExtension
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            services.AddSingleton<DbContext>();
            return services;
        }
    }
}
