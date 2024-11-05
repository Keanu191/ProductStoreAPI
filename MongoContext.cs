using MongoDB.Driver;
using WebApplicationDemoS4.Models;

namespace WebApplicationDemoS4
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            _database = client.GetDatabase(configuration["MongoDBDatabase"]);
        }

        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Product");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Category");
    }
}
