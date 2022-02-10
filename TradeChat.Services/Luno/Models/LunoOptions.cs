using System.Text.Json.Serialization;

namespace TradeChat.Services.Luno.Models
{
    public class LunoOptions
    {
        [JsonPropertyName("balanceUrl")]
        public string BalanceUrl { get; set; }
    }
}
