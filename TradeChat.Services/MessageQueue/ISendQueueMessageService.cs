using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradeChat.Services.MessageQueue
{
    public interface ISendQueueMessageService<T>
    {
        public Task SendAsync(T data);

        Task SendBatchAsync(ICollection<T> data);
    }
}
