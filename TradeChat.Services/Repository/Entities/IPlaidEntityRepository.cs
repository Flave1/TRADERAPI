using System.Threading.Tasks;
using TradeChat.Data.Entities;

namespace TradeChat.Services.Repository.Entities
{
    public interface IPlaidEntityRepository : IEntityRepository<PlaidEntity>
    {
        Task<PlaidEntity> GetAccountByItemIdAsync(string id);
    }
}
