using System.Text.Json.Serialization;

namespace TradeChat.Services.Coinbase.Models.Webhook
{
    public class CoinbaseResource
    {
        public string Id { get; set; }
        public string Resource { get; set; }

        [JsonPropertyName("resource_path")]
        public string ResourcePath { get; set; }
    }
}
