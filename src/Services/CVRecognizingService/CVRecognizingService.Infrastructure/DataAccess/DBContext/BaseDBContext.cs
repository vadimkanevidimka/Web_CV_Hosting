using Microsoft.Extensions.Options;
using MongoDB.Driver;

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
