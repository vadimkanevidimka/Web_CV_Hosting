using MongoDB.Bson;
using MongoDB.Driver;
using CVRecognizingService.Domain.Abstracts;
using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Infrastructure.DataAccess.DBMaps;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories;

public class GenericRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly IMongoClient _mdbclient;
    protected KeyValuePair<string, string> _database_collection;
    private IMongoDatabase _db;
    private IMongoCollection<T> _collection;

    public GenericRepository(IMongoClient client)
    {
        _mdbclient = client;
    }

    public async Task<long> Add(T item, CancellationToken cancellationToken)
    {
        GetDataBaseContext();
        await _collection.InsertOneAsync(item, new InsertOneOptions() { BypassDocumentValidation = true }, cancellationToken);
        return 1;
    }

    public async Task<long> Add(List<T> newitems, CancellationToken cancellationToken)
    {
        GetDataBaseContext();
        await _collection.InsertManyAsync(newitems, new InsertManyOptions() { BypassDocumentValidation = true }, cancellationToken);
        return newitems.Count;
    }

    public async Task<long> Delete(ObjectId id, CancellationToken cancellationToken)
    {
        GetDataBaseContext();
        var builder = Builders<T>.Filter;
        var deletionfilter = builder.Eq("_id", id);

        var result = await _collection.DeleteOneAsync(deletionfilter, cancellationToken);

        return result.DeletedCount;
    }

    public async Task<long> Delete(T item, CancellationToken cancellationToken)
    {
        GetDataBaseContext();
        await _collection.DeleteOneAsync(BsonDocument.Create(item), cancellationToken);
        return 1;
    }

    public async Task<T> Get(ObjectId id, CancellationToken cancellationToken)
    {
        GetDataBaseContext();
        var findfilter = new BsonDocument("_id", id);

        var a = await _collection.FindAsync<T>(findfilter);
        return await a.FirstAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken)
    {
        GetDataBaseContext();
        var elements = await _collection.FindAsync(new BsonDocument());
        return await elements.ToListAsync(cancellationToken);
    }

    public async Task<long> Update(T item, CancellationToken cancellationToken)
    {
        GetDataBaseContext();
        var findfilter = new BsonDocument("_id", item.Id);
        await _collection.ReplaceOneAsync<T>((c) => c.Id == item.Id, item, new ReplaceOptions() { IsUpsert = true }, cancellationToken);
        return 1;
    }

    private void GetDataBaseContext()
    {
        if (DataBaseMap.Map.TryGetValue(typeof(T), out _database_collection))
            _db = _mdbclient.GetDatabase(_database_collection.Key);
        _collection = _db.GetCollection<T>(_database_collection.Value);
    }
}