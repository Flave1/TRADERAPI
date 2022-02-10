using System.Text.Json.Serialization;

namespace TradeChat.Data.ViewModels.Zabo
{
    public class ZaboUserAccount
    {
        public string Id { get; set; }

        public ZaboProvider Provider { get; set; }

        [JsonPropertyName("resource_type")]
        public string ResourceType { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; }
    }

}
