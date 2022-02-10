using System.Text.Json.Serialization;

namespace TradeChat.Services.Coinbase.Models.Webhook
{
    public class CoinbaseNotificationDto
    {
        public string Id { get; set; }
        public string Type { get; set; }

        public CoinbaseNotification Data { get; set; }

        public CoinbaseResource? User { get; set; }

        public CoinbaseResource? Account { get; set; }

        [JsonPropertyName("delivery_attempts")]
        public int DeliveryAttempts { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("resource")]
        public string Resource { get; set; }

        [JsonPropertyName("resource_path")]
        public string ResourcePath { get; set; }
    }
}
