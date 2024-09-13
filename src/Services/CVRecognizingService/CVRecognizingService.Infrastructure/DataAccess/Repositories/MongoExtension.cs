using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories
{
    public static class MongoExtension
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection services, string path)
        {
            MongoClient mongoclient = new MongoClient(path);
            return services.AddScoped<IMongoClient, MongoClient>();
        }
    }
}
