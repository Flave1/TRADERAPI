using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeChat.Data.Documents;
using TradeChat.Data.ViewModels;
using TradeChat.Services.Hubs;
using TradeChat.Services.Models;
using TradeChat.Services.Repository;

namespace TradeChat.Services.ChatChannel
{
    public class ChannelService : IChannelService
    {
        private readonly IHubContext<GroupHub, IGroupClient> hubContext;
        private readonly IDocumentRepository<ChannelDocument> channelRepo;
        private readonly IDocumentRepository<UserDocument> userRepo;
        private readonly IDocumentRepository<MessageDocument> messageRepo;

        public ChannelService(
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

        public async Task<IEnumerable<ChannelDto>> ListChannels(UserClaimsInfo user)
        {
            var userDoc = await userRepo.FindByIdAsync(user.Id);
            var channels = await channelRepo.FilterByAsync(x => userDoc.Channels.Contains(x.Id));
          
            return channels.Select(x => new ChannelDto
            {
                ChannelId = x.Id,
                Name = x.DisplayName,
                LastChatTime = GetChannelLastChatTime(x.Id).Result,
                Members = x.Members.Count()
            });
        }

        public async Task<ChannelDto> CreateChannel(CreateChannelDto channel, UserClaimsInfo user)
        {
            var userDoc = await userRepo.FindByIdAsync(user.Id);
            // create a new channel record save to database
            var channelDoc = new ChannelDocument
            {
                DisplayName = channel.Name,
                Members = new List<string>() { user.Id }
            };

            userDoc.Channels.Add(channelDoc.Id);
            await channelRepo.InsertOneAsync(channelDoc);
            await userRepo.ReplaceOneAsync(userDoc);

            //add user to channel group connection
            foreach (var connectionId in userDoc.Channels)
            {
                await hubContext.Groups.AddToGroupAsync(connectionId, channelDoc.Id);
            }

            return new ChannelDto
            {
                ChannelId = channelDoc.Id,
                Name = channelDoc.DisplayName
            };
        }

        public Task DeleteChannel(string channelId, UserClaimsInfo user)
        {
            // get channel from database
            // check if user has permission to delete channel
            // update channel to deleted and move to channel archive collection
            throw new NotImplementedException();
        }

        private async Task<string> GetChannelLastChatTime(string channelId)
        {
            var lastChatTimeStampFormatted = string.Empty;
           
                var messages = await messageRepo.FilterByAsync(x => x.ChannelId == channelId);
                if (messages.Any())
                {
                    var lstMsg = messages.OrderBy(c => c.Created).LastOrDefault();
                    int timeInSeconds = 0, timeInMinutes = 0, timeInHours = 0, timeInDays = 0;

                    DateTime dtNow = DateTime.Now;
                    TimeSpan result = dtNow.Subtract(lstMsg.Created);

                    timeInSeconds = Convert.ToInt32(result.TotalSeconds);
                    timeInMinutes = Convert.ToInt32(result.TotalMinutes);
                    timeInHours = Convert.ToInt32(result.TotalHours);
                    timeInDays = Convert.ToInt32(result.TotalDays);

                    //CHECK FOR SECONDS
                    if (timeInSeconds == 1)
                    {
                        lastChatTimeStampFormatted = "a sec ago";
                    }
                    if (timeInSeconds > 1)
                    {
                        lastChatTimeStampFormatted = timeInSeconds + " secs";
                    }
                    //CHECK FOR MINUTES
                    if (timeInSeconds > 60)
                    {
                        if (timeInMinutes == 1)
                        {
                            lastChatTimeStampFormatted = "a min ago";
                        }
                        if (timeInMinutes > 1)
                        {
                            lastChatTimeStampFormatted = timeInMinutes + " mins";
                        }
                    }
                    //CHECK FOR HOURS
                    if (timeInMinutes > 60)
                    {
                        if (timeInHours == 1)
                        {
                            lastChatTimeStampFormatted = "an hour ago";
                        }
                        if (timeInHours > 1)
                        {
                            lastChatTimeStampFormatted = timeInHours + " hours";
                        }
                    }
                    //CHECK FOR DAYS
                    if (timeInHours > 60)
                    {
                        if (timeInDays == 1)
                        {
                            lastChatTimeStampFormatted = "yesterday";
                        }
                        if (timeInDays > 1)
                        {
                            lastChatTimeStampFormatted = timeInDays + " days";
                        }
                    } 
            } 
            return lastChatTimeStampFormatted == "" ? "0 secs" : lastChatTimeStampFormatted;
        }
    }
}
