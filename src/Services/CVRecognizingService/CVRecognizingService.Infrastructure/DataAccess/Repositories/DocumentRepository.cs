using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Infrastructure.DataAccess.DBContext;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories;

public class DocumentRepository : GenericRepository<Document>, IRepository<Document>
{
    public DocumentRepository(DbContext dBContext) : base(dBContext) { }

    public async Task<IEnumerable<Document>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var filter = Builders<Document>.Filter.Eq(d => d.User, userId.ToString());
        var elements = await _dbContext.GetCollection<Document>(typeof(Document).Name)
            .FindAsync<Document>(filter);
        return await elements.ToListAsync();
    }

    public async Task<long> DeleteAllAsync(CancellationToken cancellationToken)
    {
        var collection = _dbContext.GetCollection<Document>("documents");
        var filter = new BsonDocument();
        var result = await collection.DeleteManyAsync(filter, cancellationToken);

        return result.DeletedCount;
    }
}