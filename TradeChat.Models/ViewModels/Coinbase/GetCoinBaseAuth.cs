using System.Text.Json.Serialization;

namespace TradeChat.Models.ViewModels.Coinbase
{
    public class GetCoinBaseAuth
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("username")]
        public object Username { get; set; }

        [JsonPropertyName("profile_location")]
        public object ProfileLocation { get; set; }

        [JsonPropertyName("profile_bio")]
        public object ProfileBio { get; set; }

        [JsonPropertyName("profile_url")]
        public object ProfileUrl { get; set; }

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }

        [JsonPropertyName("resource")]
        public string Resource { get; set; }

        [JsonPropertyName("resource_path")]
        public string ResourcePath { get; set; }
    }
}
