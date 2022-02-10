using Newtonsoft.Json;

namespace TradeChat.Services.Luno.Models
{
    public partial class GetLunoBalance
    {
        [JsonProperty("balance")]
        public LunoBalance[] Balance { get; set; }
    }

    public partial class LunoBalance
    {
        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("balance")]
        public string BalanceBalance { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("reserved")]
        public string Reserved { get; set; }

        [JsonProperty("unconfirmed")]
        public string Unconfirmed { get; set; }
    }

}
