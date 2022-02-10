using System.Text.Json.Serialization;

namespace TradeChat.Services.Zabo.Models
{
    public class GetBrokerResponse
    {
        [JsonPropertyName("list_cursor")]
        public ListCursor CursorDetails { get; set; }

        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }

        public ZaboProviderItem[] Data { get; set; }

        public class ListCursor
        {
            public int Limit { get; set; }

            [JsonPropertyName("has_more")]
            public bool HasMore { get; set; }

            [JsonPropertyName("self_uri")]
            public string SelfUri { get; set; }

            [JsonPropertyName("next_uri")]
            public string NextUri { get; set; }
        }
    }
}
