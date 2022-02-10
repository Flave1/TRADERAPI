using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TradeChat.Models.ViewModels.Coinbase
{
    public partial class GetCoinbaseAuthData
    {
        [JsonPropertyName("data")]
        public GetCoinBaseAuth Data { get; set; }

        [JsonPropertyName("warnings")]
        public ICollection<CoinbaseAuthorizationWarning> Warnings { get; set; }
        public string Provider { get; set; }
    }




}
