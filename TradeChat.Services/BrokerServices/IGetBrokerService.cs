using System.Threading.Tasks;

namespace TradeChat.Services.BrokerServices
{
    public interface IGetBrokerService
    {
        Task GetCryptoAsync();
    }
}
