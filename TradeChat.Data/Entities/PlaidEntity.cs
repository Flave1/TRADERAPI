using System;

namespace TradeChat.Data.Entities
{
    public class PlaidEntity : BaseEntity
    {
        public string ItemId { get; set; }
        public string UserId { get; set; }
        public string AccessToken { get; set; }

        public DateTime? LastFetchDate { get; set; }
    }
}
