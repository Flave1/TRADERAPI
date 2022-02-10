using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models
{
    public class GetInvestmentTransactionDataInput
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        public string Secret { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("start_date")]
        public string StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public string EndDate { get; set; }

        public GetDataOptions Options { get; set; }

        public class GetDataOptions
        {
            public int Count { get; set; }
            public int Offset { get; set; }
        }
    }
}
