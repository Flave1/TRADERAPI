using System.Threading.Tasks;
using TradeChat.Services.Kraken.Models;

namespace TradeChat.Services.Kraken
{
    public interface IKrakenAuthenticationService
    {
        Task<KrakenAuthorizationData> TestApiKey(string key, string privateKey, string otp);
    }
}
