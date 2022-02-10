using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradeChat.Data.Documents;
using TradeChat.Data.Entities;
using TradeChat.Data.Enums;
using TradeChat.Data.MessageQueueModels;
using TradeChat.Models;
using TradeChat.Models.Options;
using TradeChat.Services.Coinbase.Models;
using TradeChat.Services.MessageQueue;
using TradeChat.Services.Repository;
using TradeChat.Services.WebRequestHelper;

namespace TradeChat.Services.Coinbase
{
    public class GetCoinbaseDataService : IGetCoinbaseDataService
    {
        private readonly ISendQueueMessageService<PostTradeQueueMessage> queueMessageService;
        private readonly IDocumentRepository<CoinbaseTransactionDocument> documentRepo;
        private readonly IEntityRepository<BrokerAccountEntity> entityRepository;
        private readonly IWebRequestService webRequestService;
        private readonly CoinbaseOptions coinbaseConfig;
        private readonly EncryptionOptions encryptionConfig;

        public GetCoinbaseDataService(
            ISendQueueMessageService<PostTradeQueueMessage> queueMessageService,
            IDocumentRepository<CoinbaseTransactionDocument> documentRepo,
            IEntityRepository<BrokerAccountEntity> entityRepository,
            IWebRequestService webRequestService,
            IOptions<CoinbaseOptions> options,
            IOptions<EncryptionOptions> encryptionOptions
        )
        {
            this.queueMessageService = queueMessageService;
            this.documentRepo = documentRepo;
            this.entityRepository = entityRepository;
            this.webRequestService = webRequestService;
            this.coinbaseConfig = options.Value;
            this.encryptionConfig = encryptionOptions.Value;
        }

        public async Task GetAsync(string transactionId, CoinbaseEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            var url = $"{coinbaseConfig.BaseUrl}/accounts/{entity.BrokerAccountId}/transactions/{transactionId}";
            var token = Encryption.AESDecrypt(encryptionConfig.BrokerAccountKey, entity.AccessToken);
            var headers = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {token}" }
            };

            var output = await webRequestService.GetAsync<GetSingleCoinbaseTransactionOutput>(url, headers);

            //map the data to a trade object
            var trade = new PostTradeQueueMessage
            {
                Amount = Decimal.Parse(output.Data.Amount.Value),
                Price = Decimal.Parse(output.Data.NativeAmount.Value),
                Quantity = Decimal.Parse(output.Data.Amount.Value),
                Currency = output.Data.NativeAmount.Currency,
                SourceItem = output.Data.NativeAmount.Currency,
                DestinationItem = output.Data.Amount.Currency,
                Type = GetTradeType(output.Data.Type),
                SecurityType = TradeSecurityType.Crypto,
                Date = output.Data.UpdatedAt,
                BrokerId = entity.BrokerId,
                UserId = entity.UserId
            };

            var transactionDoc = new CoinbaseTransactionDocument
            {
                UserId = entity.UserId,
                TransactionId = output.Data.TransactionId,
                Type = output.Data.Type,
                Status = output.Data.Status,
                Amount = output.Data.Amount.Value,
                Currency = output.Data.Amount.Currency,
                NativeAmount = output.Data.NativeAmount.Value,
                NativeCurrency = output.Data.NativeAmount.Currency,
                Description = output.Data.Description,
                CreatedAt = output.Data.CreatedAt,
                UpdatedAt = output.Data.UpdatedAt,
                ResourcePath = output.Data.ResourcePath,
                Detail = output.Data.Details.Title,
                SubDetail = output.Data.Details.Subtitle
            };

            await queueMessageService.SendAsync(trade);
            //save the transaction data as a document in mongo db
            await documentRepo.InsertOneAsync(transactionDoc);
            //update the account entity
        }


        public async Task GetAsync(int id)
        {
            var account = await entityRepository.GetAsync(id);
            if (account == null)
            {
                return;
            }

            var maxRecords = 100;
            var startingAfterTransaction = account.LastFetchIdentifier == null ? string.Empty : $"&starting_after={account.LastFetchIdentifier}";

            var url = $"{coinbaseConfig.BaseUrl}/accounts/{account.BrokerAccountId}/transactions?limit={maxRecords}{startingAfterTransaction}";
            var transactions = new List<CoinbaseTransaction>();
            var token = Encryption.AESDecrypt(encryptionConfig.BrokerAccountKey, account.AccessToken);
            var headers = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {token}" }
            };

            while (true)
            {
                var output = await webRequestService.GetAsync<GetMultipleCoinbaseTransactionsOutput>(url, headers);
                transactions.AddRange(output.Data);
                if (string.IsNullOrEmpty(output.Pagination.NextUri))
                {
                    break;
                }

                url = output.Pagination.NextUri;
            }

            //map the data to a trade object
            var trades = new List<PostTradeQueueMessage>();
            var transactionDocs = new List<CoinbaseTransactionDocument>();

            foreach (var transaction in transactions)
            {
                var trade = new PostTradeQueueMessage
                {
                    Amount = Decimal.Parse(transaction.Amount.Value),
                    Price = Decimal.Parse(transaction.NativeAmount.Value),
                    Quantity = Decimal.Parse(transaction.Amount.Value),
                    Currency = transaction.NativeAmount.Currency,
                    SourceItem = transaction.NativeAmount.Currency,
                    DestinationItem = transaction.Amount.Currency,
                    Type = GetTradeType(transaction.Type),
                    SecurityType = TradeSecurityType.Crypto,
                    Date = transaction.UpdatedAt,
                    BrokerId = account.BrokerId,
                    UserId = account.UserId
                };

                var doc = new CoinbaseTransactionDocument
                {
                    UserId = account.UserId,
                    TransactionId = transaction.TransactionId,
                    Type = transaction.Type,
                    Status = transaction.Status,
                    Amount = transaction.Amount.Value,
                    Currency = transaction.Amount.Currency,
                    NativeAmount = transaction.NativeAmount.Value,
                    NativeCurrency = transaction.NativeAmount.Currency,
                    Description = transaction.Description,
                    CreatedAt = transaction.CreatedAt,
                    UpdatedAt = transaction.UpdatedAt,
                    ResourcePath = transaction.ResourcePath,
                    Detail = transaction.Details.Title,
                    SubDetail = transaction.Details.Subtitle
                };

                transactionDocs.Add(doc);
                trades.Add(trade);
            }

            await queueMessageService.SendBatchAsync(trades);
            //save the transaction data as a document in mongo db
            await documentRepo.InsertManyAsync(transactionDocs);
            //update the account entity
        }

        private TradeType GetTradeType(string type)
        {
            switch (type)
            {
                case "send":
                case "request":
                case "transfer":
                case "fiat_deposit":
                case "exchange_deposit":
                    return TradeType.Transfer;
                case "buy":
                    return TradeType.Buy;
                case "sell":
                    return TradeType.Sell;
                case "fiat_withdrawal":
                case "exchange_withdrawal":
                case "vault_withdrawal":
                    return TradeType.Cash;
                default:
                    return TradeType.Unknown;
            }
        }
    }
}
