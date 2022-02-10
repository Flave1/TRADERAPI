using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;
using TradeChat.Data.Documents;
using TradeChat.Services.Repository;
using TradeChat.Services.UserServices;

namespace TradeChat.Services.Hubs
{
    public class GroupHub : Hub<IGroupClient>
    {
        private readonly IRetrieveUserService retrieveUserService;
        private readonly IDocumentRepository<UserDocument> userRepo;

        public GroupHub(
            IRetrieveUserService retrieveUserService,
            IDocumentRepository<UserDocument> userRepo
        )
        {
            this.retrieveUserService = retrieveUserService;
            this.userRepo = userRepo;
        }

        public override async Task OnConnectedAsync()
        {
            //get user from context
            var user = await retrieveUserService.GetUserClaimsInfo(Context.User);
            //get a list of rooms where the user belongs
            var userDoc = await userRepo.FindByIdAsync(user.Id);
            //add user to each room connection group by name
            foreach (var channelId in userDoc.Channels)
            {
                //persist a user connection record in the database for each connection
                await Groups.AddToGroupAsync(Context.ConnectionId, channelId);
            }

            userDoc.Connections.Add(Context.ConnectionId);
            await userRepo.ReplaceOneAsync(userDoc);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await retrieveUserService.GetUserClaimsInfo(Context.User);
            var userDoc = await userRepo.FindByIdAsync(user.Id);
            userDoc.Connections = userDoc.Connections.Where(x => x != Context.ConnectionId).ToList();
            await userRepo.ReplaceOneAsync(userDoc);

            //get a list of groups where the user belongs
            //remove user from each room connection group by name
            //await Groups.RemoveFromGroupAsync(Context.ConnectionId, "SignalR Users");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
