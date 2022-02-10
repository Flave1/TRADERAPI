using System.Threading.Tasks;
using TradeChat.Data.ViewModels;
using TradeChat.Services.Models;

namespace TradeChat.Services.ChatMessage
{
    public interface ISendMessageService
    {
        Task SendToChannel(string channelId, CreateMessageDto message, UserClaimsInfo user);

        Task SendToAllChannels(CreateMessageDto message, UserClaimsInfo user);

        Task SendTradeToChannels(string tradeId, string text, TradeDto trade, UserInfo user);
    }
}
