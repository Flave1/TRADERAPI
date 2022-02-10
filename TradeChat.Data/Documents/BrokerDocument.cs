using TradeChat.Data.Enums;

namespace TradeChat.Data.Documents
{
    [BsonCollection(ChatDatabaseCollections.BROKER_COLLECTION)]
    public class BrokerDocument : BaseDocument
    {
        public string DisplayName { get; set; }
        public BrokerType Type { get; set; }
        public string Provider { get; set; }
        public string Logo { get; set; }
    }
}
