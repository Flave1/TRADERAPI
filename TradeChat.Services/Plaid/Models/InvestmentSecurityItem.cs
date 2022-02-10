using System;
using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models
{
    public class InvestmentSecurityItem
    {
        [JsonPropertyName("close_price")]
        public decimal ClosePrice { get; set; }

        [JsonPropertyName("close_price_as_of")]
        public DateTime? ClosePriceAsOf { get; set; }

        [JsonPropertyName("iso_currency_code")]
        public string IsoCurrencyCode { get; set; }

        [JsonPropertyName("unofficial_currency_code")]
        public string? UnofficialCurrencyCode { get; set; }
        public string Cusip { get; set; }

        [JsonPropertyName("institution_id")]
        public string? InstitutionId { get; set; }

        [JsonPropertyName("institution_security_id")]
        public string? InstitutionSecurityId { get; set; }

        [JsonPropertyName("is_cash_equivalent")]
        public bool IsCashEquivalent { get; set; }
        public string? Isin { get; set; }
        public string Name { get; set; }

        [JsonPropertyName("proxy_security_id")]
        public string? ProxySecurityId { get; set; }

        [JsonPropertyName("security_id")]
        public string SecurityId { get; set; }
        public string? Sedol { get; set; }

        [JsonPropertyName("ticker_symbol")]
        public string TickerSymbol { get; set; }
        public string Type { get; set; }
    }
}
