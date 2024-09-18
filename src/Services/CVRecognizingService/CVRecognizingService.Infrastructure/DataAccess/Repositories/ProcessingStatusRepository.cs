using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories;
public class ProcessingStatusRepository : GenericRepository<ProcessingStatus>, IRepository<ProcessingStatus>
{
    public ProcessingStatusRepository(IMongoClient client) : base(client) { }

    public async Task<ProcessingStatus> GetDataByDocId(ObjectId id, CancellationToken cancellationToken)
    {
        GetDataBaseContext();

        var builder = Builders<ProcessingStatus>.Filter;
        var findfilter = builder.Eq("DocumentId", id);

        var data = await _collection.Find(findfilter).FirstAsync();
        return data;
    }
}