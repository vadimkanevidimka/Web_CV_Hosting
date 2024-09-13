using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using MongoDB.Driver;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories
{
    public class DocumentRepository : GenericRepository<BaseDocument>, IRepository<BaseDocument>
    {
        public DocumentRepository(IMongoClient client) : base(client) { }
    }
}
