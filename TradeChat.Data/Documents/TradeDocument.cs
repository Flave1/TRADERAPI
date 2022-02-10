using System;
using TradeChat.Data.Enums;

namespace TradeChat.Data.Documents
{
    [BsonCollection(ChatDatabaseCollections.TRADE_COLLECTION)]
    public class TradeDocument : BaseDocument
    {
        public string UserId { get; set; }
        public string BrokerId { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Fees { get; set; }
        public string DestinationItem { get; set; }
        public string SourceItem { get; set; }
        public string Currency { get; set; }
        public DateTime Date { get; set; }
        public TradeType Type { get; set; }
        public TradeSecurityType SecurityType { get; set; }
    }
}
