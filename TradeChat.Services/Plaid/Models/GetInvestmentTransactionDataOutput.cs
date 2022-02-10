using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models
{
    public class GetInvestmentTransactionDataOutput
    {
        public InvestmentOutputItem Item { get; set; }
        [JsonPropertyName("investment_transactions")]
        public ICollection<InvestmentTransactionItem> InvestmentTransactions { get; set; }
        public ICollection<InvestmentSecurityItem> Securities { get; set; }
    }
}
