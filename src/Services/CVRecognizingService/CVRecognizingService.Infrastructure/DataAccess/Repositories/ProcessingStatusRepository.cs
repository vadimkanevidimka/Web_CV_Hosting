using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Infrastructure.DataAccess.DBContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories;
public class ProcessingStatusRepository : GenericRepository<ProcessingStatus>, IRepository<ProcessingStatus>
{
    private readonly DbContext _dbContext;
    public ProcessingStatusRepository(DbContext dbContext) : base(dbContext) => _dbContext = dbContext;

    public async Task<ProcessingStatus> GetDataByDocId(ObjectId id, CancellationToken cancellationToken)
    {
        var builder = Builders<ProcessingStatus>.Filter;
        var findfilter = builder.Eq("DocumentId", id);

        var data = await _dbContext.ProcessingStatuses.Find(findfilter).FirstAsync();
        return data;
    }
}