using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeChat.Data.Documents;
using TradeChat.Data.Enums;
using TradeChat.Data.MessageQueueModels;
using TradeChat.Models.Options;
using TradeChat.Services.MessageQueue;
using TradeChat.Services.Plaid.Models;
using TradeChat.Services.Repository;
using TradeChat.Services.Repository.Entities;
using TradeChat.Services.WebRequestHelper;

namespace TradeChat.Services.Plaid
{
    public class GetPlaidDataService : IGetPlaidDataService
    {
        private readonly ISendQueueMessageService<PostTradeQueueMessage> queueMessageService;
        private readonly IDocumentRepository<PlaidInvestmentTransactionDocument> documentRepository;
        private readonly IPlaidEntityRepository plaidEntityRepository;
        private readonly IPlaidAccessTokenService accessTokenService;
        private readonly IWebRequestService webRequestService;
        private readonly PlaidOptions configOptions;
        private readonly EncryptionOptions encryptionConfig;

        public GetPlaidDataService(
            IOptions<PlaidOptions> options,
            IOptions<EncryptionOptions> encryptionOptions,
            IWebRequestService webRequestService,
            IPlaidEntityRepository plaidEntityRepository,
            IDocumentRepository<PlaidInvestmentTransactionDocument> documentRepository,
            ISendQueueMessageService<PostTradeQueueMessage> queueMessageService
            )
        {
            configOptions = options.Value;
            encryptionConfig = encryptionOptions.Value;
            this.webRequestService = webRequestService;
            this.plaidEntityRepository = plaidEntityRepository;
            this.documentRepository = documentRepository;
            this.queueMessageService = queueMessageService;
        }

        public async Task GetInvestmentTransactionAsync(int id)
        {
            var entity = await plaidEntityRepository.GetAsync(id);
            if (entity == null)
            {
                return;
            }

            var startDate = entity.LastFetchDate ?? DateTime.UtcNow.AddDays(-1);
            var endDate = DateTime.UtcNow;
            var url = $"{configOptions.BaseUrl}/investments/transactions/get";
            var maxRecords = 100;
            var transactions = new List<PlaidInvestmentTransactionDocument>();
            var token = Encryption.AESDecrypt(encryptionConfig.BrokerAccountKey, entity.AccessToken);
            while (true)
            {
                var data = new GetInvestmentTransactionDataInput
                {
                    AccessToken = token,
                    ClientId = configOptions.ClientId,
                    Secret = configOptions.Secret,
                    StartDate = startDate.ToString("yyyy-mm-dd"),
                    EndDate = endDate.ToString("yyyy-mm-dd"),
                    Options = new GetInvestmentTransactionDataInput.GetDataOptions
                    {
                        Count = maxRecords,
                        Offset = transactions.Count
                    }
                };

                var result = await webRequestService.PostAsync<GetInvestmentTransactionDataOutput, GetInvestmentTransactionDataInput>(url, data);
                transactions.AddRange(result.InvestmentTransactions.Select(x =>
                {
                    var security = result.Securities.FirstOrDefault(s => s.SecurityId == x.SecurityId);
                    return new PlaidInvestmentTransactionDocument
                    {
                        AccountId = x.AccountId,
                        Amount = x.Amount,
                        SecurityId = x.SecurityId,
                        ClosePriceAsOf = security.ClosePriceAsOf ?? null,
                        CancelTransactionId = x.CancelTransactionId ?? null,
                        ClosePrice = security.ClosePrice,
                        Cusip = security.Cusip,
                        Date = x.Date,
                        Fees = x.Fees,
                        InstitutionId = security.InstitutionId ?? null,
                        InstitutionSecurityId = security.InstitutionSecurityId ?? null,
                        InvestmentTransactionId = x.InvestmentTransactionId,
                        IsCashEquivalent = security.IsCashEquivalent,
                        Isin = security.Isin,
                        IsoCurrencyCode = x.IsoCurrencyCode,
                        ProxySecurityId = security.ProxySecurityId,
                        ItemId = result.Item.ItemId,
                        UserId = entity.UserId,
                        UnofficialCurrencyCode = security.UnofficialCurrencyCode,
                        Name = x.Name,
                        SecurityName = security.Name,
                        SecurityType = security.Type,
                        Sedol = security.Sedol,
                        Subtype = x.Subtype,
                        TickerSymbol = security.TickerSymbol,
                        Type = x.Type,
                        Price = x.Price,
                        Quantity = x.Quantity
                    };
                }));

                if (result.InvestmentTransactions.Count < maxRecords)
                {
                    break;
                }
            }

            var trades = new List<PostTradeQueueMessage>();
            foreach (var transaction in transactions)
            {
                var trade = new PostTradeQueueMessage
                {
                    Amount = transaction.Amount,
                    Price = transaction.Price,
                    Fees = transaction.Fees,
                    Quantity = transaction.Quantity,
                    Currency = transaction.IsoCurrencyCode,
                    SourceItem = transaction.IsoCurrencyCode,
                    DestinationItem = transaction.SecurityName,
                    Type = GetTradeType(transaction.Type),
                    SecurityType = GetSecurityType(transaction.SecurityType),
                    Date = transaction.Date,
                    BrokerId = transaction.InstitutionId,
                    UserId = transaction.UserId
                };

                trades.Add(trade);
            }

            await queueMessageService.SendBatchAsync(trades);

            entity.LastFetchDate = endDate;
            await documentRepository.InsertManyAsync(transactions);
            await plaidEntityRepository.UpdateAsync(entity);
        }

        private TradeType GetTradeType(string type)
        {
            switch (type)
            {
                case "buy":
                    return TradeType.Buy;
                case "sell":
                    return TradeType.Sell;
                case "cancel":
                    return TradeType.Cancel;
                case "cash":
                    return TradeType.Cash;
                case "fee":
                    return TradeType.Fee;
                case "transfer":
                    return TradeType.Transfer;
                default:
                    return TradeType.Unknown;
            }
        }

        private TradeSecurityType GetSecurityType(string type)
        {
            switch (type)
            {
                case "cash":
                    return TradeSecurityType.Cash;
                case "derivative":
                    return TradeSecurityType.Options;
                case "equity":
                    return TradeSecurityType.Equity;
                case "etf":
                    return TradeSecurityType.ETF;
                case "mutual fund":
                    return TradeSecurityType.MutualFund;
                default:
                    return TradeSecurityType.Unknown;
            }
        }
    }
}
