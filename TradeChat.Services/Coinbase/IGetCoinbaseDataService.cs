using System.Threading.Tasks;
using TradeChat.Data.Entities;

namespace TradeChat.Services.Coinbase
{
    public interface IGetCoinbaseDataService
    {
        Task GetAsync(int id);

        Task GetAsync(string transactionId, CoinbaseEntity entity);
    }
}
