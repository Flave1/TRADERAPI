using System.Threading.Tasks;
using TradeChat.Data.Entities;

namespace TradeChat.Services.Repository.Entities
{
    public interface ICoinbaseEntityRepository : IEntityRepository<CoinbaseEntity>
    {
        Task<CoinbaseEntity> FindByBrokerAccount(string accountId);
    }
}
