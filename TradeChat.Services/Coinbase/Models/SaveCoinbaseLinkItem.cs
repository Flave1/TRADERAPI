namespace TradeChat.Services.Coinbase.Models
{
    public class SaveCoinbaseLinkItem
    {
        public string UserId { get; set; }
        public string CoinbaseUserId { get; set; }
        public string AccessToken { get; set; }
        public string BrokerId { get; set; }
    }
}
