using MongoDB.Driver;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Domain.Abstracts.Repo;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories;
public class ProcessingLogRepository : GenericRepository<ProcessingLog>, IRepository<ProcessingLog>
{
    public ProcessingLogRepository(IMongoClient client) : base(client)
    {
    }
}