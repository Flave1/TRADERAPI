using MongoDB.Driver;
using TradeChat.Data;

namespace TradeChat.Services
{
    public class DatabaseInitializationService : IDatabaseInitializationService
    {
        private IMongoDatabase _database;
        public DatabaseInitializationService(IChatDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            _database = client.GetDatabase(settings.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
