using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models
{
    public class GetPlaidAccessTokenInput
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        public string Secret { get; set; }
        [JsonPropertyName("public_token")]
        public string PublicToken { get; set; }
    }
}
