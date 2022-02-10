using System.Threading.Tasks;

namespace TradeChat.Services.Plaid
{
    public interface IGetPlaidInstitutionService
    {
        Task LoadAsync();
    }
}
