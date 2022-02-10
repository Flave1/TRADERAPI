using System.Collections.Generic;

namespace TradeChat.Data.Documents
{
    [BsonCollection(ChatDatabaseCollections.USER_COLLECTION)]
    public class UserDocument : BaseDocument
    {
        public string ProfilePictureUrl { get; set; }
        public string UserName { get; set; }
        public ICollection<string> Channels { get; set; }
        public ICollection<string> Connections { get; set; }
        public ICollection<string> Brokers { get; set; }

        public UserDocument() : base()
        {
            Channels = new List<string>();
            Connections = new List<string>();
            Brokers = new List<string>();
        }
    }
}
