using System.Security.Claims;
using TradeChat.Services.Extensions;

namespace TradeChat.Services.Models
{
    public class UserClaimsInfo
    {
        public readonly string Id;
        public readonly string Name;
        public readonly string Email;

        internal UserClaimsInfo(ClaimsPrincipal claims)
        {
            this.Id = claims.Identity.Id();
            this.Name = claims.Identity.Name();
            this.Email = claims.Identity.Email();
        }
    }
}
