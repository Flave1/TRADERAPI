using System.Text.Json.Serialization;

namespace TradeChat.Models.ViewModels.Coinbase
{
    public class GetCoinbaseInput
    {
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }

        public string code { get; set; }

        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }

        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; }
    }
}
