using MongoDB.Bson;
using MongoDB.Driver;
using CVRecognizingService.Domain.Abstracts;
using CVRecognizingService.Domain.Abstracts.Repo;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly KeyValuePair<string,string> _database_collection;
        private readonly IMongoDatabase _db;
        private readonly IMongoClient _mdbclient;
        private readonly IMongoCollection<T> _collection;

        public GenericRepository(IMongoClient client)
        {
            _mdbclient = client;
            
            if (DataBaseMap.Map.TryGetValue(typeof(T), out _database_collection))
                _db = _mdbclient.GetDatabase(_database_collection.Key);
            _collection = _db.GetCollection<T>(_database_collection.Value);
        }

        public async Task<long> Add(T item, CancellationToken cancellationToken)
        {
            await _collection.InsertOneAsync(item, new InsertOneOptions() { BypassDocumentValidation = true }, cancellationToken);
            return 1;
        }

        public async Task<long> Add(List<T> newitems, CancellationToken cancellationToken)
        {
            await _collection.InsertManyAsync(newitems, new InsertManyOptions() { BypassDocumentValidation = true }, cancellationToken);
            return newitems.Count;
        }

        public async Task<long> Delete(ObjectId id, CancellationToken cancellationToken)
        {
            var builder = Builders<T>.Filter;
            var deletionfilter = builder.Eq("Id", $"{id}");

            var result = await _collection.DeleteOneAsync(deletionfilter, cancellationToken);

            return result.DeletedCount;
        }

        public async Task<long> Delete(T item, CancellationToken cancellationToken)
        {
            await _collection.DeleteOneAsync(BsonDocument.Create(item), cancellationToken);
            return 1;
        }

        public async Task<T> Get(ObjectId id, CancellationToken cancellationToken)
        {
            var builder = Builders<T>.Filter;
            var findfilter = builder.Eq("Id", $"{id}");

            var a = await _collection.FindAsync<T>(findfilter);
            return await a.FirstAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken)
        {
            var elements = await _collection.FindAsync(new BsonDocument());
            return await elements.ToListAsync(cancellationToken);
        }

        public async Task<long> Update(T item, CancellationToken cancellationToken)
        {
            var findfilter = new BsonDocument("Id",$"{item.Id}");

            var result = await _collection.ReplaceOneAsync(findfilter, item, new ReplaceOptions { IsUpsert = true }, cancellationToken);
            return result.ModifiedCount;
        }
    }
}
