using System.Text.Json.Serialization;

namespace TradeChat.Data.ViewModels.Zabo
{
    public class ZaboProvider
    {
        public string Name { get; set; }

        public string Logo { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [JsonPropertyName("auth_type")]
        public string AuthType { get; set; }

        public string[] Scopes { get; set; }

        [JsonPropertyName("resource_type")]
        public string ResourceType { get; set; }
    }
}
