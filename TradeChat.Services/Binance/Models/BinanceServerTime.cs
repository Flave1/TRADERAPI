using Newtonsoft.Json;

namespace TradeChat.Services.Binance.Models
{
    public class BinanceServerTime
    {
        [JsonProperty("serverTime")]
        public long ServerTime { get; set; }
    }
}
