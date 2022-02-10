using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models.Webhook
{
    public class PlaidInvestmentHoldingsWebhook : PlaidWebhookItem
    {
        [JsonPropertyName("new_holdings")]
        public int NewHoldings { get; set; }

        [JsonPropertyName("updated_holdings")]
        public int UpdatedHoldings { get; set; }
    }
}
