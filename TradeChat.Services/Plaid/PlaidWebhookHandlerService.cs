using System.Text.Json;
using System.Threading.Tasks;
using TradeChat.Data.Enums;
using TradeChat.Data.MessageQueueModels;
using TradeChat.Services.MessageQueue;
using TradeChat.Services.Plaid.Constants;
using TradeChat.Services.Plaid.Models.Webhook;
using TradeChat.Services.Repository.Entities;

namespace TradeChat.Services.Plaid
{
    public class PlaidWebhookHandlerService : IPlaidWebhookHandlerService
    {
        private readonly IPlaidEntityRepository repository;
        private readonly ISendQueueMessageService<PlaidInvestmentQueueMessage> messageService;

        public PlaidWebhookHandlerService(
            IPlaidEntityRepository repository,
            ISendQueueMessageService<PlaidInvestmentQueueMessage> messageService)
        {
            this.repository = repository;
            this.messageService = messageService;
        }

        public async Task RunAsync(string type, string content)
        {
            //save and post a trade/portfolio item from plaid
            switch (type)
            {
                case WebhookTypeConstants.InvestmentTransactions:
                    var item = JsonSerializer.Deserialize<PlaidInvestmentTransactionWebhook>(content);
                    await HandleInvestmentTransactionUpdate(item);
                    break;
            }
        }

        private async Task HandleInvestmentTransactionUpdate(PlaidInvestmentTransactionWebhook item)
        {
            // check for error
            if (item.NewTransactions < 1 && item.CancelledTransactions < 1)
            {
                return;
            }

            // identify account in database as needing an update
            var account = await repository.GetAccountByItemIdAsync(item.ItemId);
            if (account == null)
            {
                return;
            }

            var message = new PlaidInvestmentQueueMessage
            {
                // avoid sending sensitive information across the network
                Id = account.Id,
                Type = PlaidInvestmentQueueMessageType.Investment
            };

            //make a call to azure service bus to page data
            await messageService.SendAsync(message);
        }
    }
}
