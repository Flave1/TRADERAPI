using MongoDB.Driver;

namespace TradeChat.Services
{
    public interface IDatabaseInitializationService
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
