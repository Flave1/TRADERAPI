using System.Threading.Tasks;
using TradeChat.Services.Binance.Models;

namespace TradeChat.Services.Binance
{
    public interface IBinanceAuthenticationService
    {
        Task<string> GetAuthorizationUrl();
        Task<BinanceOAuthAuthorizationData> GetTokens(string code);
        Task<BinanceAuthorizationData> TestApiKey(string apiKey, string apiSecret);
    }
}
