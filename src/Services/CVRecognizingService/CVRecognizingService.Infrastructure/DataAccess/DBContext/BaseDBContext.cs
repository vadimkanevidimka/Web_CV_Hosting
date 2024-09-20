using CVRecognizingService.Domain.Abstracts;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Diagnostics.Contracts;

namespace CVRecognizingService.Infrastructure.DataAccess.DBContext
{
    public abstract class BaseDBContext
    {
        protected readonly IMongoDatabase _db;

        public BaseDBContext(IOptions<ConnectionSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _db = client.GetDatabase(options.Value.Database);
        }
    }
}
