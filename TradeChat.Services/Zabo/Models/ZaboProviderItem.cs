using System.Text.Json.Serialization;

namespace TradeChat.Services.Zabo.Models
{
    public class ZaboProviderItem
    {
        public string Name { get; set; }

        [JsonPropertyName("display_name")]
        public string Displayname { get; set; }

        [JsonPropertyName("auth_type")]
        public string AuthType { get; set; }
        public string Logo { get; set; }

        [JsonPropertyName("resource_type")]
        public string ResourceType { get; set; }

        [JsonPropertyName("available_currencies")]
        public AvailableCurrencyItem[] AvailableCurrencies { get; set; }

        //public string? Status { get; set; }

        [JsonPropertyName("available_scopes")]
        public ScopeItem[] AvailableScopes { get; set; }

        [JsonPropertyName("is_beta")]
        public bool IsBeta { get; set; }

        [JsonPropertyName("connect_notice")]
        public string ConnectNotice { get; set; }

        [JsonPropertyName("status_notice")]
        public string StatusNotice { get; set; }

        public class AvailableCurrencyItem
        {
            public string Type { get; set; }
            public string[] List { get; set; }

            [JsonPropertyName("resource_type")]
            public string ResourceType { get; set; }
        }

        public class ScopeItem
        {
            public string Name { get; set; }

            [JsonPropertyName("display_name")]
            public string DisplayName { get; set; }

            public string Description { get; set; }
        }
    }
}
