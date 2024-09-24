using CVRecognizingService.Infrastructure.DataAccess.DBContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CVRecognizingService.Application.ServiceExctensions
{
    public static class DBConnectionSettingExtension
    {
        public static IServiceCollection AddDbConnectionSettings(
            this IServiceCollection services, 
            IConfiguration configuration) 
        {
            return services.Configure<ConnectionSettings>(
                options =>
                {
                    options.ConnectionString = configuration.GetSection("MongoDb:ConnectionString").Value;
                    options.Database = configuration.GetSection("MongoDb:Database").Value;
                });
        }
    }
}
