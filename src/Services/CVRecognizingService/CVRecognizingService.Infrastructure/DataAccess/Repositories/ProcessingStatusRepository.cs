using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using MongoDB.Driver;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories;
public class ProcessingStatusRepository : GenericRepository<ProcessingStatus>, IRepository<ProcessingStatus>
{
    public ProcessingStatusRepository(IMongoClient client) : base(client) { }
}