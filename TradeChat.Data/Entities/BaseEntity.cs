using System;

namespace TradeChat.Data.Entities
{
    public class BaseEntity : IEntity
    {
        public int Id { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public bool Archived { get; set; } = false;
    }
}
