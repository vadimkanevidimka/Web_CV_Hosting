using MongoDB.Bson;
using MongoDB.Driver;
using CVRecognizingService.Domain.Abstracts;
using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Infrastructure.DataAccess.DBContext;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories;

public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : IEntity
{
   private readonly DbContext _dbContext;

    public GenericRepository(DbContext dBContext)
    {
        _dbContext = dBContext;
    }

    public async Task<long> AddAsync(TEntity item, CancellationToken cancellationToken)
    {

        var a = _dbContext.GetCollection<TEntity>(typeof(TEntity).Name);
        await a.InsertOneAsync(item, new InsertOneOptions() { BypassDocumentValidation = true }, cancellationToken);
        return 1;
    }

    public async Task<long> AddRangeAsync(List<TEntity> newitems, CancellationToken cancellationToken)
    {
        await _dbContext.GetCollection<TEntity>(typeof(TEntity).Name)
            .InsertManyAsync(newitems, new InsertManyOptions() { BypassDocumentValidation = true }, cancellationToken);
        return newitems.Count;
    }

    public async Task<long> DeleteAsync(ObjectId id, CancellationToken cancellationToken)
    {
        
        var builder = Builders<TEntity>.Filter;
        var deletionfilter = builder.Eq("_id", id);

        var result = await _dbContext.GetCollection<TEntity>(typeof(TEntity).Name)
            .DeleteOneAsync(deletionfilter, cancellationToken);

        return result.DeletedCount;
    }

    public async Task<long> DeleteAsync(TEntity item, CancellationToken cancellationToken)
    {
        
        await _dbContext.GetCollection<TEntity>(typeof(TEntity).Name)
            .DeleteOneAsync(BsonDocument.Create(item), cancellationToken);
        return 1;
    }

    public async Task<TEntity> GetByIdAsync(ObjectId id, CancellationToken cancellationToken)
    {
        
        var findfilter = new BsonDocument("_id", id);

        var a = await _dbContext.GetCollection<TEntity>(typeof(TEntity).Name)
            .FindAsync<TEntity>(findfilter);
        return await a.FirstAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
    {
        
        var elements = await _dbContext.GetCollection<TEntity>(typeof(TEntity).Name)
            .FindAsync(new BsonDocument());
        return elements.ToEnumerable<TEntity>();
    }

    public async Task<long> UpdateAsync(TEntity item, CancellationToken cancellationToken)
    {
        await _dbContext.GetCollection<TEntity>(typeof(TEntity).Name)
            .ReplaceOneAsync((c) => c.Id == item.Id, item, new ReplaceOptions() { IsUpsert = true }, cancellationToken);
        return 1;
    }
}