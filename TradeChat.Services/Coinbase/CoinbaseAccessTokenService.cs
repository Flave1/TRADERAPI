using System.Threading.Tasks;
using TradeChat.Data.Entities;
using TradeChat.Services.Coinbase.Models;
using TradeChat.Services.Repository.Entities;

namespace TradeChat.Services.Coinbase
{
    public class CoinbaseAccessTokenService : ICoinbaseAccessTokenService
    {
        private readonly ICoinbaseEntityRepository entityRepository;

        public CoinbaseAccessTokenService(ICoinbaseEntityRepository entityRepository)
        {
            this.entityRepository = entityRepository;
        }

        public async Task SaveAsync(SaveCoinbaseLinkItem item)
        {
            var entity = new CoinbaseEntity
            {
                AccessToken = item.AccessToken,
                BrokerAccountId = item.CoinbaseUserId,
                BrokerId = item.BrokerId,
                UserId = item.UserId
            };

            await entityRepository.AddAsync(entity);
        }
    }
}
