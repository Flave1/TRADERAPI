using System.Threading.Tasks;
using TradeChat.Services.Coinbase.Models.Webhook;

namespace TradeChat.Services.Coinbase
{
    public interface ICoinbaseNotificationService
    {
        Task RunAsync(CoinbaseNotificationDto notificationDto);
    }
}
