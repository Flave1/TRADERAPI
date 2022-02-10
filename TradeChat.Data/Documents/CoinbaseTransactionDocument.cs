using System;

namespace TradeChat.Data.Documents
{
    [BsonCollection(ChatDatabaseCollections.COINBASE_TRANSACTION_COLLECTION)]
    public class CoinbaseTransactionDocument : BaseDocument
    {
        public string UserId { get; set; }

        public string TransactionId { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public string Amount { get; set; }

        public string Currency { get; set; }

        public string NativeAmount { get; set; }

        public string NativeCurrency { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string ResourcePath { get; set; }

        public string Detail { get; set; }

        public string SubDetail { get; set; }

    }
}
