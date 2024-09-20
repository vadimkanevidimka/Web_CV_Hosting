using CVRecognizingService.Domain.Abstracts.Repo;
using CVRecognizingService.Domain.Entities;
using CVRecognizingService.Infrastructure.DataAccess.DBContext;

namespace CVRecognizingService.Infrastructure.DataAccess.Repositories;

public class DocumentRepository : GenericRepository<Document>, IRepository<Document>
{
    public DocumentRepository(DbContext dBContext) : base(dBContext) { }
}