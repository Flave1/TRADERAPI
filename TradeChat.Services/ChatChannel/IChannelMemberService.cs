using System.Collections.Generic;
using System.Threading.Tasks;
using TradeChat.Data.ViewModels;
using TradeChat.Services.Models;

namespace TradeChat.Services.ChatChannel
{
    public interface IChannelMemberService
    {
        Task<ICollection<ChannelMemberDto>> ListMembers(string channelId);

        Task LeaveChannel(string channelId, UserClaimsInfo user);

        Task AddToChannel(string channelId, UserClaimsInfo user);

        Task RemoveFromChannel(string channelId, string targetUserId, UserClaimsInfo user);
    }
}
