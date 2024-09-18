using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CVRecognizingService.Application.ServiceExctensions;

public static class MongoExtension
{
    public static IServiceCollection AddMongoDB(this IServiceCollection services, string path)
    {
        services.AddSingleton<IMongoClient>(s => new MongoClient(path));
        return services;
    }
}
