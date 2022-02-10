using System.Security.Claims;
using System.Threading.Tasks;
using TradeChat.Services.Models;

namespace TradeChat.Services.UserServices
{
    public interface IRetrieveUserService
    {
        Task<UserClaimsInfo> GetUserClaimsInfo(ClaimsPrincipal claims);

        Task<UserInfo> GetUserAsync(string userId);
    }
}
