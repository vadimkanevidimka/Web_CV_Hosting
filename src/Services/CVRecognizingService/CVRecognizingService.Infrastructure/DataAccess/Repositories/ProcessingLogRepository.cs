using MongoDB.Driver;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Domain.Abstracts.Repo;
using MongoDB.Bson;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories;
public class ProcessingLogRepository : GenericRepository<ProcessingLog>, IRepository<ProcessingLog>
{
    public ProcessingLogRepository(IMongoClient client) : base(client) {}
    public async Task<ProcessingLog> GetDataByDocId(ObjectId id, CancellationToken cancellationToken)
    {
        GetDataBaseContext();

        var builder = Builders<ProcessingLog>.Filter;
        var findfilter = builder.Eq("DocumentId", id);

        var data = await _collection.Find(findfilter).FirstAsync();
        return data;
    }
}