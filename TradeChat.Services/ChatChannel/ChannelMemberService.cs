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

namespace TradeChat.Services.ChatChannel
{
    public class ChannelMemberService : IChannelMemberService
    {
        private readonly IHubContext<GroupHub, IGroupClient> hubContext;
        private readonly IDocumentRepository<MessageDocument> messageRepo;
        private readonly IDocumentRepository<ChannelDocument> channelRepo;
        private readonly IDocumentRepository<UserDocument> userRepo;

        public ChannelMemberService(
            IHubContext<GroupHub, IGroupClient> hubContext,
            IDocumentRepository<UserDocument> userRepo,
            IDocumentRepository<ChannelDocument> channelRepo,
            IDocumentRepository<MessageDocument> messageRepo)
        {
            this.hubContext = hubContext;
            this.userRepo = userRepo;
            this.channelRepo = channelRepo;
            this.messageRepo = messageRepo;
        }

        public async Task AddToChannel(string channelId, UserClaimsInfo user)
        {
            //get channel by id
            var channel = await channelRepo.FindByIdAsync(channelId);
            var userDoc = await userRepo.FindByIdAsync(user.Id);
            //get user id
            //add user to channel
            if (!channel.Members.Contains(user.Id))
            {
                channel.Members.Add(user.Id);
            }

            if (!userDoc.Channels.Contains(channelId))
            {
                userDoc.Channels.Add(channelId);
            }

            await channelRepo.ReplaceOneAsync(channel);
            await userRepo.ReplaceOneAsync(userDoc);

            //add user to channel connection group
            if (userDoc.Connections != null)
            {
                foreach (var connectionId in userDoc.Connections)
                {
                    await hubContext.Groups.AddToGroupAsync(connectionId, channelId);
                }
            }

            //create a new notification message for group (channel) that new user has joined
            var messageDoc = new MessageDocument
            {
                UserId = user.Id,
                UserName = user.Name,
                ChannelId = channelId,
                Text = $"{user.Name} has joined the channel.",
                Type = MessageType.Notification,
            };

            await messageRepo.InsertOneAsync(messageDoc);
            //broadcast a message that a new user has been added to channel
            await hubContext.Clients.Group(channelId).ReceiveMessage(new MessageItemDto
            {
                Text = messageDoc.Text,
                TimeStamp = messageDoc.Created,
                UserName = user.Name,
                Type = MessageType.Notification,
                Id = messageDoc.Id
            });
        }

        public async Task LeaveChannel(string channelId, UserClaimsInfo user)
        {
            var channelDoc = await channelRepo.FindByIdAsync(channelId);
            if (channelDoc == null)
            {
                return;
            }

            var userDoc = await userRepo.FindByIdAsync(user.Id);
            userDoc.Channels = userDoc.Channels.Where(x => x != channelId).ToList();
            channelDoc.Members = channelDoc.Members.Where(x => x != user.Id).ToList();

            await userRepo.ReplaceOneAsync(userDoc);
            await channelRepo.ReplaceOneAsync(channelDoc);

            //remove user from group subscription
            foreach (var connectionId in userDoc.Connections)
            {
                await hubContext.Groups.RemoveFromGroupAsync(connectionId, channelId);
            }

            //broadcast that user has left channel
            //create a new notification message for group (channel) that new user has joined
            var messageDoc = new MessageDocument
            {
                UserId = user.Id,
                UserName = user.Name,
                ChannelId = channelId,
                Text = $"{user.Name} has left the channel.",
                Type = MessageType.Notification,
            };

            await messageRepo.InsertOneAsync(messageDoc);
            //broadcast a message that a new user has been added to channel
            await hubContext.Clients.Group(channelId).ReceiveMessage(new MessageItemDto
            {
                Text = messageDoc.Text,
                TimeStamp = messageDoc.Created,
                UserName = messageDoc.UserName,
                Id = messageDoc.Id,
                Type = MessageType.Notification
            });
        }

        public async Task<ICollection<ChannelMemberDto>> ListMembers(string channelId)
        {
            var channelDoc = await channelRepo.FindByIdAsync(channelId);
            var memberDocs = await userRepo.FilterByAsync(x => channelDoc.Members.Contains(x.Id));
            return memberDocs.Select(x => new ChannelMemberDto
            {
                Id = x.Id,
                Name = x.UserName,
                Joined = x.Created
            }).ToList();
        }

        public Task RemoveFromChannel(string channelId, string targetUserId, UserClaimsInfo user)
        {
            // get channel from database
            // check if user has permission to remove from channel
            // remove user from channel
            throw new NotImplementedException();
        }
    }
}
