using System.Threading.Tasks;
using TradeChat.Data.ViewModels;

namespace TradeChat.Services.Hubs
{
    public interface IGroupClient
    {
        Task ReceiveMessage(MessageItemDto message);
    }
}
