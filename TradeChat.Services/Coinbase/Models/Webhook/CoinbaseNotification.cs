using System.Text.Json.Serialization;

namespace TradeChat.Services.Coinbase.Models.Webhook
{
    public class CoinbaseNotification
    {
        public string Id { get; set; }

        public string Status { get; set; }

        [JsonPropertyName("payment_method")]
        public CoinbaseResource? PaymentMethod { get; set; }

        public CoinbaseResource? Transaction { get; set; }

        public CoinbaseCurrencyValue? Amount { get; set; }

        public CoinbaseCurrencyValue? Total { get; set; }

        public CoinbaseCurrencyValue? Subtotal { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; }

        public string Resource { get; set; }

        [JsonPropertyName("resource_path")]
        public string ResourcePath { get; set; }

        public bool? Committed { get; set; }

        public bool? Instant { get; set; }

        public CoinbaseCurrencyValue? Fee { get; set; }

        [JsonPropertyName("payout_at")]
        public string? PayoutAt { get; set; }
    }

}
