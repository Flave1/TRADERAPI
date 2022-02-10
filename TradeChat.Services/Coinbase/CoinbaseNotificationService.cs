using System.Threading.Tasks;
using TradeChat.Services.Coinbase.Models.Webhook;
using TradeChat.Services.Repository.Entities;

namespace TradeChat.Services.Coinbase
{
    public class CoinbaseNotificationService : ICoinbaseNotificationService
    {
        private readonly IGetCoinbaseDataService getCoinbaseDataService;
        private readonly ICoinbaseEntityRepository entityRepository;

        public CoinbaseNotificationService(
            IGetCoinbaseDataService getCoinbaseDataService,
            ICoinbaseEntityRepository entityRepository
        )
        {
            this.getCoinbaseDataService = getCoinbaseDataService;
            this.entityRepository = entityRepository;
        }

        public async Task RunAsync(CoinbaseNotificationDto notificationDto)
        {
            var userAccount = notificationDto.User.Id;
            // get user account details from database
            var entity = await entityRepository.FindByBrokerAccount(userAccount);
            if (entity == null)
            {
                return;
            }

            // get transaction id from notification dto
            var transactionId = notificationDto.Data.Transaction.Id;
            await getCoinbaseDataService.GetAsync(transactionId, entity);
        }
    }
}
