using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models.Webhook
{
    public class PlaidWebhookError
    {
        [JsonPropertyName("error_type")]
        public string ErrorType { get; set; }

        [JsonPropertyName("error_code")]
        public string ErrorCode { get; set; }

        [JsonPropertyName("error_message")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("display_message")]
        public string DisplayMessage { get; set; }

        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }
        public ICollection<string> Causes { get; set; }
        public int Status { get; set; }

        [JsonPropertyName("documentation_url")]
        public string DocumentationUrl { get; set; }

        [JsonPropertyName("suggested_action")]
        public string SuggestedAction { get; set; }
    }
}
