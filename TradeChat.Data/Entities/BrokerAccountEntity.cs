using System;

namespace TradeChat.Data.Entities
{
    public class BrokerAccountEntity : BaseEntity
    {
        public string UserId { get; set; }
        public string BrokerId { get; set; }
        public string BrokerAccountId { get; set; }
        public string AccessToken { get; set; }
        public string AccountKey { get; set; }
        public string AccountSecret { get; set; }
        public string BrokerName { get; set; }
        public DateTime? LastFetchDate { get; set; }
        public string LastFetchIdentifier { get; set; }

        public int RequestBatchId { get; set; }
        public RequestBatchEntity RequestBatch { get; set; }
    }
}
