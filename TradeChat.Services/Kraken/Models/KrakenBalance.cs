using System.Text.Json.Serialization;

namespace TradeChat.Services.Kraken.Models
{

    public partial class GetKrakenBalance
    {
        [JsonPropertyName("error")]
        public dynamic[] Error { get; set; }

        [JsonPropertyName("result")]
        public KrakenBalaceResult Result { get; set; }
    }



}
