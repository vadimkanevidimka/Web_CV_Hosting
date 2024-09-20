using MongoDB.Driver;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Domain.Abstracts.Repo;
using MongoDB.Bson;
using CVRecognizingService.Infrastructure.DataAccess.DBContext;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories;
public class ProcessingLogRepository : GenericRepository<ProcessingLog>, IRepository<ProcessingLog>
{
    private readonly DbContext _dbContext;
    public ProcessingLogRepository(DbContext dbContext) : base(dbContext) => _dbContext = dbContext;
    public async Task<ProcessingLog> GetDataByDocId(ObjectId id, CancellationToken cancellationToken)
    {
        var builder = Builders<ProcessingLog>.Filter;
        var findfilter = builder.Eq("DocumentId", id);

        var data = await _dbContext.ProcessingLogs.Find(findfilter).FirstAsync();
        return data;
    }
}