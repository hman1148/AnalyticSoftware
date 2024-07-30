using AnalyticSoftware.Models;
using MongoDB.Driver;

namespace AnalyticSoftware.Database
{
    public class DatabaseContext
    {

        private readonly IMongoDatabase _mongoDatabase;

        public DatabaseContext(MongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _mongoDatabase = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<User> Users => _mongoDatabase.GetCollection<User>("Users");
    }
}
