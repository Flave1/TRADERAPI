using System.Threading.Tasks;
using TradeChat.Data.ViewModels;

namespace TradeChat.Services.TradeServices
{
    public interface IShareTradeService
    {
        Task PostAsync(TradeDto trade);
    }
}
