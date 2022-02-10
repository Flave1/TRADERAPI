using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using TradeChat.Data.ViewModels;

namespace TradeChat.Services.Hubs
{
    [Authorize]
    public class PrivateHub : Hub
    {
        public Task SendMessage(CreateMessageDto message)
        {
            return Clients.All.SendAsync("ReceivedMessage", message);
        }
    }
}
