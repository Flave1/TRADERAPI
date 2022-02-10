using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeChat.Data.Documents;
using TradeChat.Data.Enums;
using TradeChat.Data.ViewModels;
using TradeChat.Services.Hubs;
using TradeChat.Services.Models;
using TradeChat.Services.Repository;

namespace TradeChat.Services.ChatMessage
{
    public class SendMessageService : ISendMessageService
    {
        private readonly IHubContext<GroupHub, IGroupClient> hubContext;
        private readonly IDocumentRepository<MessageDocument> messageRepo;
        private readonly IDocumentRepository<UserDocument> userRepo;

        public SendMessageService(
            IHubContext<GroupHub, IGroupClient> hubContext,
            IDocumentRepository<MessageDocument> messageRepo,
            IDocumentRepository<UserDocument> userRepo
        )
        {
            this.hubContext = hubContext;
            this.messageRepo = messageRepo;
            this.userRepo = userRepo;
        }

        private async Task SendMultiple(MessageItemDto message, ICollection<MessageDocument> documents)
        {
            await messageRepo.InsertManyAsync(documents);
            await hubContext.Clients.Groups(documents.Select(x => x.ChannelId).ToArray()).ReceiveMessage(message);
        }

        public async Task SendTradeToChannels(string tradeId, string text, TradeDto trade, UserInfo user)
        {
            //get all user channels
            var userDoc = await userRepo.FindByIdAsync(user.Id);
            if (userDoc.Channels == null || userDoc.Channels.Count < 1)
                return;

            var documents = userDoc.Channels.Select(channelId => new MessageDocument
            {
                Text = text,
                ChannelId = channelId,
                Type = MessageType.Regular,
                UserId = user.Id,
                UserName = user.UserName,
                TradeId = tradeId
            }).ToList();

            var messageItem = new MessageItemDto
            {
                Text = text,
                TimeStamp = DateTime.UtcNow,
                Type = MessageType.Regular,
                UserName = user.UserName
            };

            await SendMultiple(messageItem, documents);
        }

        public async Task SendToAllChannels(CreateMessageDto message, UserClaimsInfo user)
        {
            //get all user channels
            var userDoc = await userRepo.FindByIdAsync(user.Id);
            if (userDoc.Channels == null || userDoc.Channels.Count < 1)
                return;

            var documents = userDoc.Channels.Select(channelId => new MessageDocument
            {
                Text = message.Text,
                ChannelId = channelId,
                Type = MessageType.Regular,
                UserId = user.Id,
                UserName = user.Name
            }).ToList();

            var messageItem = new MessageItemDto
            {
                Text = message.Text,
                TimeStamp = DateTime.UtcNow,
                Type = MessageType.Regular,
                UserName = user.Name
            };

            await SendMultiple(messageItem, documents);
        }

        public async Task SendToChannel(string channelId, CreateMessageDto message, UserClaimsInfo user)
        {
            var userDoc = await userRepo.FindByIdAsync(user.Id);
            //verify that user can send to channel
            if (userDoc.Channels == null || userDoc.Channels.Count < 1 || !userDoc.Channels.Contains(channelId))
                return;

            var messageDoc = new MessageDocument
            {
                Text = message.Text,
                ChannelId = channelId,
                Type = MessageType.Regular,
                UserId = user.Id,
                UserName = user.Name
            };

            await messageRepo.InsertOneAsync(messageDoc);
            await hubContext.Clients.Group(channelId).ReceiveMessage(new MessageItemDto
            {
                Id = messageDoc.Id,
                Text = message.Text,
                TimeStamp = messageDoc.Created,
                Type = messageDoc.Type,
                UserName = user.Name
            });
        }
    }
}
