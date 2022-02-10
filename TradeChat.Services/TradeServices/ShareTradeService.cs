using System.Threading.Tasks;
using TradeChat.Data.Documents;
using TradeChat.Data.Enums;
using TradeChat.Data.ViewModels;
using TradeChat.Services.ChatMessage;
using TradeChat.Services.Repository;
using TradeChat.Services.UserServices;

namespace TradeChat.Services.TradeServices
{
    public class ShareTradeService : IShareTradeService
    {
        private readonly IDocumentRepository<TradeDocument> tradeRepo;
        private readonly ISendMessageService sendMessageService;
        private readonly IRetrieveUserService retrieveUserService;

        public ShareTradeService(
            IDocumentRepository<TradeDocument> tradeRepo,
            ISendMessageService sendMessageService,
            IRetrieveUserService retrieveUserService
        )
        {
            this.tradeRepo = tradeRepo;
            this.sendMessageService = sendMessageService;
            this.retrieveUserService = retrieveUserService;
        }

        public async Task PostAsync(TradeDto trade)
        {
            var tradeDoc = new TradeDocument
            {
                Amount = trade.Amount,
                BrokerId = trade.BrokerId,
                Currency = trade.Currency,
                Date = trade.Date,
                DestinationItem = trade.DestinationItem,
                SourceItem = trade.SourceItem,
                Fees = trade.Fees,
                Price = trade.Price,
                Quantity = trade.Quantity,
                Type = trade.Type,
                SecurityType = trade.SecurityType,
                UserId = trade.UserId
            };

            await tradeRepo.InsertOneAsync(tradeDoc);
            if (!CanPostTrade(trade.Type))
            {
                return;
            }

            var user = await retrieveUserService.GetUserAsync(trade.UserId);
            var messageText = GetTradeTypeMessage(trade);
            await sendMessageService.SendTradeToChannels(tradeDoc.Id, messageText, trade, user);
        }

        private bool CanPostTrade(TradeType type)
        {
            switch (type)
            {
                case TradeType.Buy:
                case TradeType.Sell:
                case TradeType.Cancel:
                    return true;
                default:
                    return false;
            }
        }

        private string GetTradeTypeMessage(TradeDto trade)
        {
            switch (trade.SecurityType)
            {
                case TradeSecurityType.Cash:
                case TradeSecurityType.Crypto:
                    switch (trade.Type)
                    {
                        case TradeType.Buy:
                            return $"{trade.Quantity} ({trade.DestinationItem}) bought for {trade.Price} ({trade.Currency})";
                        case TradeType.Sell:
                            return $"{trade.Quantity} ({trade.DestinationItem}) sold for {trade.Price} ({trade.Currency})";
                        case TradeType.Cancel:
                            return $"Trade action of {trade.Quantity}({trade.DestinationItem}) at {trade.Price} ({trade.Currency}) was cancelled";
                        default:
                            return $"Trade action of {trade.Quantity}({trade.DestinationItem}) at {trade.Price} ({trade.Currency}) taken on account";
                    }
                case TradeSecurityType.Equity:
                    switch (trade.Type)
                    {
                        case TradeType.Buy:
                            return $"{trade.Quantity} shares of {trade.DestinationItem} bought for {trade.Price} ({trade.Currency})";
                        case TradeType.Sell:
                            return $"{trade.Quantity} shares of {trade.DestinationItem} sold for {trade.Price} ({trade.Currency})";
                        case TradeType.Cancel:
                            return $"Trade action of {trade.Quantity}({trade.DestinationItem}) at {trade.Price} ({trade.Currency}) was cancelled";
                        default:
                            return $"Trade action of {trade.Quantity}({trade.DestinationItem}) at {trade.Price} ({trade.Currency}) taken on account";
                    }
                case TradeSecurityType.Options:
                    switch (trade.Type)
                    {
                        case TradeType.Cancel:
                            return $"Option exercise of {trade.Quantity}({trade.DestinationItem}) at {trade.Price} ({trade.Currency}) was cancelled";
                        default:
                            return $"Option of {trade.Quantity}({trade.DestinationItem}) at {trade.Price} ({trade.Currency}) exercised on account";
                    }
                case TradeSecurityType.ETF:
                case TradeSecurityType.MutualFund:
                    switch (trade.Type)
                    {
                        case TradeType.Buy:
                            return $"Buy action of {trade.Quantity}({trade.DestinationItem}) at {trade.Price} ({trade.Currency}) taken on account";
                        case TradeType.Sell:
                            return $"Sell action of {trade.Quantity}({trade.DestinationItem}) at {trade.Price} ({trade.Currency}) taken on account";
                        case TradeType.Cancel:
                            return $"Trade action of {trade.Quantity}({trade.DestinationItem}) at {trade.Price} ({trade.Currency}) was cancelled";
                        default:
                            return $"Trade action of {trade.Quantity}({trade.DestinationItem}) at {trade.Price} ({trade.Currency}) taken on account";
                    }
                default:
                    return $"Action taken on account Quantity: {trade.Quantity}, Item: {trade.DestinationItem}, Price: {trade.Price}, Currency: {trade.Currency}";
            }
        }
    }
}
