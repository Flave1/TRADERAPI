namespace TradeChat.Data.Documents
{
    [BsonCollection(ChatDatabaseCollections.PLAID_LINK_COLLECTION)]
    public class PlaidLinkDocument : BaseDocument
    {
        public string PublicToken { get; set; }
        public string ItemId { get; set; }
        public string UserId { get; set; }
        public string RequestId { get; set; }
    }
}
