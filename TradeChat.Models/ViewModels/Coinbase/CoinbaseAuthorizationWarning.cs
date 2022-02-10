using System.Text.Json.Serialization;

namespace TradeChat.Models.ViewModels.Coinbase
{
    public partial class CoinbaseAuthorizationWarning
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
