using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using MongoDB.Driver;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories;
public class ProcessedDataRepository : GenericRepository<ProcessedData>, IRepository<ProcessedData>
{
    public ProcessedDataRepository(IMongoClient client) : base(client)
    {
    }
}