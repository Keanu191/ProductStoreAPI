using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace WebApplicationDemoS4.Data
{
    public class MongoContext
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoDatabase? _database;

        public MongoContext(IConfiguration configuration)
        {
            _configuration = configuration;

            var connectionString = _configuration.GetConnectionString("DbConnection");
            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }

        public IMongoDatabase? Database => _database;
    }
}
