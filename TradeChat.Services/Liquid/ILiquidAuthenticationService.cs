using System.Threading.Tasks;
using TradeChat.Services.Liquid.Models;

namespace TradeChat.Services.Liquid
{
    public interface ILiquidAuthenticationService
    {
        Task<LiquidAuthorizationData> TestApiKey(string apiTokenId, string apiSecret);
    }
}
