using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models.Webhook
{
    public class PlaidWebhookItem
    {
        [JsonPropertyName("webhook_type")]
        public string? WebhookType { get; set; }

        [JsonPropertyName("webhook_code")]
        public string? WebhookCode { get; set; }

        [JsonPropertyName("item_id")]
        public string? ItemId { get; set; }
        public PlaidWebhookError? Error { get; set; }
    }
}
