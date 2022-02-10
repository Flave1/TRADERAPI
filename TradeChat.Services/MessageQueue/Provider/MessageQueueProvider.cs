using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using TradeChat.Services.MessageQueue.Configuration;

namespace TradeChat.Services.MessageQueue.Provider
{
    public class MessageQueueProvider : IMessageQueueProvider
    {
        private Dictionary<string, ServiceBusSender> senders;

        public MessageQueueProvider(IOptions<MessageQueueConfigOptions> options)
        {
            senders = new Dictionary<string, ServiceBusSender>();

            var configuration = options.Value;
            var serviceBusClient = new ServiceBusClient(configuration.ConnectionString);
            var plaidPagingQueue = serviceBusClient.CreateSender(configuration.PlaidInvestmentQueue);
            senders.Add(configuration.PlaidInvestmentQueue, plaidPagingQueue);

            var postTradeQueue = serviceBusClient.CreateSender(configuration.PostTradeDataQueue);
            senders.Add(configuration.PostTradeDataQueue, postTradeQueue);
        }

        public ServiceBusSender Resolve(string queueName)
        {
            return senders[queueName];
        }
    }
}
