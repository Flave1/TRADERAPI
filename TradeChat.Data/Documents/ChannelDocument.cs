using System.Collections.Generic;

namespace TradeChat.Data.Documents
{
    [BsonCollection(ChatDatabaseCollections.CHANNEL_COLLECTION)]
    public class ChannelDocument : BaseDocument
    {
        public string DisplayName { get; set; }

        public string LogoUrl { get; set; }

        public ICollection<string> Members { get; set; }
    }
}
