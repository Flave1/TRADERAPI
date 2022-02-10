using System.Threading.Tasks;
using TradeChat.Services.Luno.Models;

namespace TradeChat.Services.Luno
{
    public interface ILunoAuthenticationService
    {
        Task<LunoAuthorizationData> TestApiKey(string apiKeyId, string apiKeySecret);
    }
}
