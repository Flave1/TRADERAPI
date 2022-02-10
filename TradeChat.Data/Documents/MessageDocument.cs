using TradeChat.Data.Enums;

namespace TradeChat.Data.Documents
{
    [BsonCollection(ChatDatabaseCollections.MESSAGE_COLLECTION)]
    public class MessageDocument : BaseDocument
    {
        public MessageType Type { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string ChannelId { get; set; }
        public string TradeId { get; set; }
        public string FileUrl { get; set; }
    }
}
