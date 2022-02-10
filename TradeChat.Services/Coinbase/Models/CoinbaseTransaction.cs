using System;
using System.Text.Json.Serialization;

namespace TradeChat.Services.Coinbase.Models
{
    public class CoinbaseTransaction
    {
        [JsonPropertyName("id")]
        public string TransactionId { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }

        public TransactionAmount Amount { get; set; }

        [JsonPropertyName("native_amount")]
        public TransactionAmount NativeAmount { get; set; }

        public string Description { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public string Resource { get; set; }

        [JsonPropertyName("resource_path")]
        public string ResourcePath { get; set; }

        public BuyDetails Buy { get; set; }

        public TransactionDetails Details { get; set; }

        public class TransactionAmount
        {
            [JsonPropertyName("amount")]
            public string Value { get; set; }
            public string Currency { get; set; }
        }

        public class BuyDetails
        {
            public string Id { get; set; }
            public string Resource { get; set; }

            [JsonPropertyName("resource_path")]
            public string ResourcePath { get; set; }
        }

        public class TransactionDetails
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
        }
    }
}
