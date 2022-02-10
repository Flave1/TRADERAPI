using System.Threading.Tasks;
using TradeChat.Services.Plaid.Models;

namespace TradeChat.Services.Plaid
{
    public interface IPlaidAccessTokenService
    {
        Task SaveAsync(SavePlaidLinkItem item);
    }
}
