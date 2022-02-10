namespace TradeChat.Data.Documents
{
    [BsonCollection(ChatDatabaseCollections.LINKED_BROKER_COLLECTION)]
    public class LinkedBrokerDocument : BaseDocument
    {
        public string BrokerId { get; set; }
        public string UserId { get; set; }
        public string BrokerUserAccountId { get; set; }
        public string Key { get; set; }
    }
}
