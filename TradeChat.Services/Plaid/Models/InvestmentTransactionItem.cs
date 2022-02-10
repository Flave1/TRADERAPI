using System;
using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models
{
    public class InvestmentTransactionItem
    {
        [JsonPropertyName("account_id")]
        public string AccountId { get; set; }
        public decimal Amount { get; set; }

        [JsonPropertyName("cancel_transaction_id")]
        public string? CancelTransactionId { get; set; }
        public DateTime Date { get; set; }
        public decimal Fees { get; set; }

        [JsonPropertyName("investment_transaction_id")]
        public string InvestmentTransactionId { get; set; }

        [JsonPropertyName("iso_currency_code")]
        public string IsoCurrencyCode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }

        [JsonPropertyName("security_id")]
        public string SecurityId { get; set; }
        public string Subtype { get; set; }
        public string Type { get; set; }

        [JsonPropertyName("unofficial_currency_code")]
        public string? UnofficialCurrencyCode { get; set; }
    }
}
