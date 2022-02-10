using System.Threading.Tasks;
using TradeChat.Models.ViewModels;
using TradeChat.Services.Models;

namespace TradeChat.Services.ChatChannel
{
    public interface IChannelInvitationService
    {
        Task InviteAsync(InviteMemberRequest request, UserClaimsInfo user);

        Task RedeemInvitationAsync(string code, UserClaimsInfo user);

    }
}
