using TradeChat.Data.Enums;

namespace TradeChat.Data.MessageQueueModels
{
    public class PlaidInvestmentQueueMessage
    {
        public int Id { get; set; }
        public PlaidInvestmentQueueMessageType Type { get; set; }
    }
}
