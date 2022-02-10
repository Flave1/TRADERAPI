namespace TradeChat.Services.Plaid.Models
{
    public class PlaidOptions
    {
        public string BaseUrl { get; set; }
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string ClientName { get; set; }
        public string WebhookUrl { get; set; }
    }
}
