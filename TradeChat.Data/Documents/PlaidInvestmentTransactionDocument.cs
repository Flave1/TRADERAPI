using System;

namespace TradeChat.Data.Documents
{
    [BsonCollection(ChatDatabaseCollections.PLAID_INVESTMENT_TRANSACTION_COLLECTION)]
    public class PlaidInvestmentTransactionDocument : BaseDocument
    {
        public string ItemId { get; set; }
        public string UserId { get; set; }

        public string AccountId { get; set; }
        public decimal Amount { get; set; }
        public string? CancelTransactionId { get; set; }
        public DateTime Date { get; set; }
        public decimal Fees { get; set; }
        public string InvestmentTransactionId { get; set; }
        public string IsoCurrencyCode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public string SecurityId { get; set; }
        public string Subtype { get; set; }
        public string Type { get; set; }
        public string? UnofficialCurrencyCode { get; set; }

        //Security Information
        public decimal ClosePrice { get; set; }
        public DateTime? ClosePriceAsOf { get; set; }
        public string Cusip { get; set; }
        public string? InstitutionId { get; set; }
        public string? InstitutionSecurityId { get; set; }
        public bool IsCashEquivalent { get; set; }
        public string? Isin { get; set; }
        public string SecurityName { get; set; }
        public string? ProxySecurityId { get; set; }
        public string? Sedol { get; set; }
        public string TickerSymbol { get; set; }
        public string SecurityType { get; set; }
    }
}
