using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models
{
    public class GetPlaidAccessTokenOutput
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("item_id")]
        public string ItemId { get; set; }
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }
    }
}
