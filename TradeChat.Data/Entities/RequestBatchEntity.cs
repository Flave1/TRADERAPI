using System;
using System.Collections.Generic;

namespace TradeChat.Data.Entities
{
    public class RequestBatchEntity : BaseEntity
    {
        public DateTime LastFetchTime { get; set; }
        public int MaxItemCount { get; set; }

        public ICollection<BrokerAccountEntity> BrokerAccounts { get; set; }
    }
}
