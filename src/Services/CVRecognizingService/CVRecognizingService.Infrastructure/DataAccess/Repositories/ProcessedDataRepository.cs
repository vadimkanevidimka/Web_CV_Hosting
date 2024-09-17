using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories;
public class ProcessedDataRepository : GenericRepository<ProcessedData>, IRepository<ProcessedData>
{
    public ProcessedDataRepository(IMongoClient client) : base(client) {}

    public async Task<ProcessedData> GetDataByDocId(ObjectId id, CancellationToken cancellationToken)
    {
        GetDataBaseContext();

        var builder = Builders<ProcessedData>.Filter;
        var findfilter = builder.Eq("DocumentId", id);

        var data = await _collection.Find(findfilter).FirstAsync();
        return data;
    }
}