namespace TradeChat.Services.Binance.Models
{
    public class BinanceOptions
    {
        public string AccountStatusUrl { get; set; }
        public string AuthorizeUrl { get; set; }
        public string TokensUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scopes { get; set; }
        public string CallbackUrl;
        public string AuthorizeSuccessUrl { get; set; }
        public string AuthorizeFailUrl { get; set; }
    }
}
