using Microsoft.Extensions.Options;
using TradeChat.Data.MessageQueueModels;
using TradeChat.Services.MessageQueue.Configuration;
using TradeChat.Services.MessageQueue.Provider;

namespace TradeChat.Services.MessageQueue
{
    public class PostTradeQueueMessageService : SendQueueMessageService<PostTradeQueueMessage>
    {
        public PostTradeQueueMessageService(IOptions<MessageQueueConfigOptions> options, IMessageQueueProvider queueProvider)
        {
            sender = queueProvider.Resolve(options.Value.PostTradeDataQueue);
        }
    }
}
