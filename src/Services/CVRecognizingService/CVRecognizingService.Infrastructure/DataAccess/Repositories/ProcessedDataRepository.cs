using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Infrastructure.DataAccess.DBContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories;
public class ProcessedDataRepository : GenericRepository<ProcessedData>, IRepository<ProcessedData>
{
    private readonly DbContext _dbContext;
    public ProcessedDataRepository(DbContext dbContext) : base(dbContext) => _dbContext = dbContext;

    public async Task<ProcessedData> GetDataByDocId(ObjectId id, CancellationToken cancellationToken)
    {
        var builder = Builders<ProcessedData>.Filter;
        var findfilter = builder.Eq("DocumentId", id);

        var data = await _dbContext.ProcessedDatas.Find(findfilter).FirstAsync();
        return data;
    }
}