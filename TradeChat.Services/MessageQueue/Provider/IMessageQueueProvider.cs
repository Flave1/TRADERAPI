using Azure.Messaging.ServiceBus;

namespace TradeChat.Services.MessageQueue.Provider
{
    public interface IMessageQueueProvider
    {
        ServiceBusSender Resolve(string queueName);
    }
}
