using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models
{
    internal class CreateLinkInput
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        [JsonPropertyName("client_name")]
        public string ClientName { get; set; }
        public string Secret { get; set; }
        public string[] Products { get; set; }
        [JsonPropertyName("country_codes")]
        public string[] CountryCodes { get; set; }
        public string Language { get; set; }
        public string Webhook { get; set; }
        public ClientUser User { get; set; }

        public class ClientUser
        {
            [JsonPropertyName("client_user_id")]
            public string ClientUserId { get; set; }
        }
    }
}
