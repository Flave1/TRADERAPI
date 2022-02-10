using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeChat.Data.Documents;
using TradeChat.Data.ViewModels;
using TradeChat.Services.Repository;

namespace TradeChat.Services.ChatMessage
{
    public class RetrieveMessageService : IRetrieveMessageService
    {
        private readonly IDocumentRepository<MessageDocument> messageRepo;
        public RetrieveMessageService(IDocumentRepository<MessageDocument> messageRepo)
        {
            this.messageRepo = messageRepo;
        }

        public async Task<ICollection<MessageItemDto>> ListChannelMessages(string channelId)
        {
            var messages = await messageRepo.FilterByAsync(x => x.ChannelId == channelId);
             
            return messages.OrderBy(x => x.Created).Select(x => new MessageItemDto
            {
                Id = x.Id,
                Text = x.Text,
                TimeStamp = x.Created,
                Type = x.Type,
                UserName = x.UserName, 
            }).ToList();
        }

       
    }
}
