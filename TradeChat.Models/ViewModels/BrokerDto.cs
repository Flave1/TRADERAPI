using TradeChat.Data.Enums;

namespace TradeChat.Data.ViewModels
{
    public class BrokerDto
    {
        public string DisplayName { get; set; }
        public string Id { get; set; }
        public BrokerType Type { get; set; }
        public string Provider { get; set; }
        public string Logo { get; set; }
    }
}
