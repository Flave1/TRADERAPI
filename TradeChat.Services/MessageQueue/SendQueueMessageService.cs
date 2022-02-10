using Azure.Messaging.ServiceBus;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace TradeChat.Services.MessageQueue
{
    public class SendQueueMessageService<TQueueMessage> : ISendQueueMessageService<TQueueMessage>
        where TQueueMessage : class
    {
        protected ServiceBusSender sender;

        public virtual async Task SendAsync(TQueueMessage data)
        {
            var content = JsonSerializer.Serialize(data);
            ServiceBusMessage message = new ServiceBusMessage(content);
            await sender.SendMessageAsync(message);
        }

        public virtual async Task SendBatchAsync(ICollection<TQueueMessage> data)
        {
            var messages = data.Select(x =>
            {
                var content = JsonSerializer.Serialize(x);
                return new ServiceBusMessage(content);
            });

            await sender.SendMessagesAsync(messages);
        }
    }
}
