using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models
{
    internal class InstitutionItem
    {
        [JsonPropertyName("country_codes")]
        public string[] CountryCodes { get; set; }

        [JsonPropertyName("institution_id")]
        public string InstitutionId { get; set; }
        public string Name { get; set; }
        public bool Oauth { get; set; }
        public string Logo { get; set; }
        public string Url { get; set; }
        public string[] Products { get; set; }

        [JsonPropertyName("routing_numbers")]
        public string[] RoutingNumbers { get; set; }
    }
}
