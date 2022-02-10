using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models.Webhook
{
    public class PlaidInvestmentTransactionWebhook : PlaidWebhookItem
    {
        [JsonPropertyName("new_investments_transactions")]
        public int NewTransactions { get; set; }

        [JsonPropertyName("canceled_investments_transactions")]
        public int CancelledTransactions { get; set; }
    }
}
