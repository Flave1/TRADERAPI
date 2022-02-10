using System.Threading.Tasks;
using TradeChat.Services.Coinbase.Models;
using TradeChat.Services.Models;

namespace TradeChat.Services.Coinbase
{
    public interface ICoinbaseAuthorizationService
    {
        public Task<string> GetAuthorizationUrl();
        public Task<CoinbaseAuthorizationData> GetTokens(string code);
        Task SaveUserCoinBaseData(string code, UserClaimsInfo user);
    }

}
