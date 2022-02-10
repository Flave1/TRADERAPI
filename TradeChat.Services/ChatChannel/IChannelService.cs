using System.Collections.Generic;
using System.Threading.Tasks;
using TradeChat.Data.ViewModels;
using TradeChat.Services.Models;

namespace TradeChat.Services.ChatChannel
{
    public interface IChannelService
    {
        Task<IEnumerable<ChannelDto>> ListChannels(UserClaimsInfo user);

        Task<ChannelDto> CreateChannel(CreateChannelDto channel, UserClaimsInfo user);

        Task DeleteChannel(string channelId, UserClaimsInfo user);
    }
}
