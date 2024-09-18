using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Infrastructure.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CVRecognizingService.Application.ServiceExctensions;
public static class RepositoriesExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRepository<BaseDocument>, DocumentRepository>();
        services.AddScoped<IRepository<ProcessingStatus>, ProcessingStatusRepository>();
        services.AddScoped<IRepository<ProcessedData>, ProcessedDataRepository>();
        services.AddScoped<IRepository<ProcessingLog>, ProcessingLogRepository>();
        return services;
    }
}