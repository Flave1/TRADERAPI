using System.Threading.Tasks;

namespace TradeChat.Services.Plaid
{
    public interface IGetPlaidDataService
    {
        Task GetInvestmentTransactionAsync(int id);
    }
}
