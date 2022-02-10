namespace TradeChat.Services.Coinbase.Models
{
    public class CoinbaseOptions
    {
        public string BaseUrl { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string AuthorizeUrl { get; set; }

        public string TokenUrl { get; set; }

        public string Scopes { get; set; }

        public string CallbackUrl { get; set; }

        public string UserProfile { get; set; }
    }
}
