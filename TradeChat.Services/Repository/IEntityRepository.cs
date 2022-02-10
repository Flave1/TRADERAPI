using System.Collections.Generic;
using System.Threading.Tasks;
using TradeChat.Data.Entities;

namespace TradeChat.Services.Repository
{
    public interface IEntityRepository<T> where T : class, IEntity
    {
        Task<List<T>> GetAll();
        Task<T> GetAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(int id);
    }
}
