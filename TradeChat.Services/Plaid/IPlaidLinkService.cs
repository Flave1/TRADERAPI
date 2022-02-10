using System.Threading.Tasks;
using TradeChat.Services.Models;

namespace TradeChat.Services.Plaid
{
    public interface IPlaidLinkService
    {
        Task<string> GetLinkTokenAsync(UserClaimsInfo user);

        Task<string> SaveItemAsync(string publicToken, UserClaimsInfo user);
    }
}
