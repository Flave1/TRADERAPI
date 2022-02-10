using System.Threading.Tasks;

namespace TradeChat.Auth.Services
{
    public interface IAccessTokenService
    {
        Task<string> GetAsync();
    }
}
