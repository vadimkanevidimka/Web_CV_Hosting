using CSharpFunctionalExtensions;
using CVRecognizingService.Domain.Abstracts;
using CVRecognizingService.Domain.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CVRecognizingService.Infrastructure.DataAccess.DBContext
{
    public class DbContext : BaseDBContext
    {
        public DbContext(IOptions<ConnectionSettings> options) : base(options) { }
        public IMongoCollection<Document> Documents => _db.GetCollection<Document>(typeof(Document).Name);
        public IMongoCollection<ProcessedData> ProcessedDatas => _db.GetCollection<ProcessedData>(typeof(ProcessedData).Name);
        public IMongoCollection<ProcessingLog> ProcessingLogs => _db.GetCollection<ProcessingLog>(typeof(ProcessingLog).Name);
        public IMongoCollection<ProcessingStatus> ProcessingStatuses => _db.GetCollection<ProcessingStatus>(typeof(ProcessingStatus).Name);
        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _db.GetCollection<T>(name);
        }
    }
}
