namespace TradeChat.Services.MessageQueue.Configuration
{
    public class MessageQueueConfigOptions
    {
        public string ConnectionString { get; set; }
        public string PlaidInvestmentQueue { get; set; }
        public string PostTradeDataQueue { get; set; }
    }
}
