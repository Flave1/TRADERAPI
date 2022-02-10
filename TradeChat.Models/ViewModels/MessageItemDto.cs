using System;
using TradeChat.Data.Enums;

namespace TradeChat.Data.ViewModels
{
    public class MessageItemDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string Text { get; set; }

        public DateTime TimeStamp { get; set; }

        public MessageType Type { get; set; }
    }
}
