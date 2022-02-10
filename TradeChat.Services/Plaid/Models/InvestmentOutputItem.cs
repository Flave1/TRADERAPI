using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models
{
    public class InvestmentOutputItem
    {
        [JsonPropertyName("item_id")]
        public string ItemId { get; set; }

        [JsonPropertyName("institution_id")]
        public string InstitutionId { get; set; }
    }
}
