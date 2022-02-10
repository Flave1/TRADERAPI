using System.Threading.Tasks;

namespace TradeChat.Services.Plaid
{
    public interface IPlaidWebhookHandlerService
    {
        Task RunAsync(string type, string content);
    }
}
