using System;
using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models
{
    public class CreateLinkOutput
    {
        [JsonPropertyName("link_token")]
        public string LinkToken { get; set; }
        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }
        public DateTime Expiration { get; set; }
    }
}
