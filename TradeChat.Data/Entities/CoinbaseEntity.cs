namespace TradeChat.Data.Entities
{
    public class CoinbaseEntity : BaseEntity
    {
        public string UserId { get; set; }
        public string BrokerId { get; set; }
        public string BrokerAccountId { get; set; }
        public string LastTransactionId { get; set; }
        public string AccessToken { get; set; }
    }
}
