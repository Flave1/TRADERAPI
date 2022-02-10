using System.Collections.Generic;
using System.Threading.Tasks;
using TradeChat.Data.ViewModels;
using TradeChat.Data.ViewModels.Plaid;
using TradeChat.Data.ViewModels.Zabo;
using TradeChat.Models.ViewModels;
using TradeChat.Models.ViewModels.Coinbase;
using TradeChat.Services.Models;

namespace TradeChat.Services.BrokerServices
{
    public interface ILinkedBrokerService
    {
        Task<ICollection<BrokerDto>> GetAsync(UserClaimsInfo user);

        Task LinkAsync(ZaboUserAccount account, UserClaimsInfo user);

        Task LinkAsync(SavePlaidUserAccount account, UserClaimsInfo user);
        Task LinkAsync(GetCoinbaseAuthData account, UserClaimsInfo user);
        Task LinkAsync(ProviderKeyDto account, UserClaimsInfo user);
    }
}
