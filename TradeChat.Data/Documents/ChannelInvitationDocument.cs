using System;

namespace TradeChat.Data.Documents
{
    [BsonCollection(ChatDatabaseCollections.CHANNEL_INVITATION_COLLECTION)]
    public class ChannelInvitationDocument : BaseDocument
    {
        public string ChannelId { get; set; }
        public string InvitedUserEmail { get; set; }
        public string TriggerUserId { get; set; }
        public string InvitationCode { get; set; }
        public DateTime? Expiry { get; set; }
    }
}
