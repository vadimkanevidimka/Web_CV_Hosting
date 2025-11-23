using CVRecognizingService.Domain.Abstracts;
using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Infrastructure.DataAccess.DBContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories;

public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : IEntity
{
    protected readonly DbContext _dbContext;

    public GenericRepository(DbContext dBContext)
    {
        _dbContext = dBContext;
    }

    public async Task<long> AddAsync(TEntity item, CancellationToken cancellationToken)
    {

        await _dbContext.GetCollection<TEntity>(typeof(TEntity).Name)
            .InsertOneAsync(item, new InsertOneOptions() { BypassDocumentValidation = true }, cancellationToken);
        return 1;
    }

    public async Task<long> AddAsync(List<TEntity> newitems, CancellationToken cancellationToken)
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

        var result = await _dbContext.GetCollection<TEntity>(typeof(TEntity).Name)
            .DeleteOneAsync(BsonDocument.Create(item), cancellationToken);
        return result.DeletedCount;
    }

    public async Task<TEntity> GetAsync(ObjectId id, CancellationToken cancellationToken)
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
        return await elements.ToListAsync();
    }

    public async Task<long> UpdateAsync(TEntity item, CancellationToken cancellationToken)
    {
        var result = await _dbContext.GetCollection<TEntity>(typeof(TEntity).Name)
            .ReplaceOneAsync((c) => c.Id == item.Id, item, new ReplaceOptions() { IsUpsert = true }, cancellationToken);
        return result.ModifiedCount;
    }
}