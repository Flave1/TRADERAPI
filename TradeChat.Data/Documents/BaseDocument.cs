using System;

namespace TradeChat.Data.Documents
{
    public class BaseDocument : IDocument
    {
        public string Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public bool Deleted { get; set; }

        public BaseDocument()
        {
            Id = Guid.NewGuid().ToString();
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
            Deleted = false;
        }
    }
}
