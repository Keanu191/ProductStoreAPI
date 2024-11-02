
/*
 * 30074191 / Keanu Farro
 * Code used from youtube tutorial: https://www.youtube.com/watch?v=Gxf7zBl5Z64
 */
using MongoDB.Driver;

namespace WebApplicationDemoS4.Data
{
    public class MongoDBService
    {
        private readonly IConfiguration _configuration;
        private readonly IMongoDatabase? _database;

        public MongoDBService(IConfiguration configuration)
        {
            _configuration = configuration;

            var connectionString = _configuration.GetConnectionString("ConnectionString");
            var mongoURL = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoURL);
            _database = mongoClient.GetDatabase(mongoURL.DatabaseName);
        }

        public IMongoDatabase? Database => _database;
    }
}
