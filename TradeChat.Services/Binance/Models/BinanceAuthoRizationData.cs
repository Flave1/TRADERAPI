using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace TradeChat.Services.Binance.Models
{
    public class BinanceAuthorizationData
    {
        /// <summary>
        /// API Key
        /// </summary>
        /// <value></value>
        public string ApiKey { get; set; }

        /// <summary>
        /// API Secret
        /// </summary>
        /// <value></value>
        public string ApiSecret { get; set; }

        /// <summary>
        /// API Status
        /// </summary>
        /// <value></value>
        [JsonProperty("code")]
        public int Code { get; set; }

        /// <summary>
        /// API Status Messgae
        /// </summary>
        /// <value></value>
        [JsonProperty("msg")]
        public string Msg { get; set; } 
    }


}
