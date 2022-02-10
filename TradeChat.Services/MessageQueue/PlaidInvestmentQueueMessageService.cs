using Microsoft.Extensions.Options;
using TradeChat.Data.MessageQueueModels;
using TradeChat.Services.MessageQueue.Configuration;
using TradeChat.Services.MessageQueue.Provider;

namespace TradeChat.Services.MessageQueue
{
    public class PlaidInvestmentQueueMessageService : SendQueueMessageService<PlaidInvestmentQueueMessage>
    {
        public PlaidInvestmentQueueMessageService(IOptions<MessageQueueConfigOptions> options, IMessageQueueProvider queueProvider)
        {
            sender = queueProvider.Resolve(options.Value.PlaidInvestmentQueue);
        }
    }
}
