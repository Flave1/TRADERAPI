using System.Threading.Tasks;
using TradeChat.Services.Coinbase.Models;

namespace TradeChat.Services.Coinbase
{
    public interface ICoinbaseAccessTokenService
    {
        Task SaveAsync(SaveCoinbaseLinkItem item);
    }
}
