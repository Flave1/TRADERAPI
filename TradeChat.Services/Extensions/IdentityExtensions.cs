using System.Security.Claims;
using System.Security.Principal;
namespace TradeChat.Services.Extensions
{
    public static class IdentityExtensions
    {
        public static string Name(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaimTypes.Name);

            return claim?.Value ?? string.Empty;
        }

        public static string Email(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(ClaimTypes.Email);

            return claim?.Value ?? string.Empty;
        }

        public static string Id(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(UserClaimTypes.Id);

            return claim?.Value ?? string.Empty;
        }
    }
}
