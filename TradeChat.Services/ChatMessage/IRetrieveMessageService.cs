using System.Collections.Generic;
using System.Threading.Tasks;
using TradeChat.Data.ViewModels;

namespace TradeChat.Services.ChatMessage
{
    public interface IRetrieveMessageService
    {
        Task<ICollection<MessageItemDto>> ListChannelMessages(string channelId);
    }
}
