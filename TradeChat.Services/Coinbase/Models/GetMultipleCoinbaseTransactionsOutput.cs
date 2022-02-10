using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TradeChat.Services.Coinbase.Models
{
    public class GetMultipleCoinbaseTransactionsOutput
    {
        public PaginationDetails Pagination { get; set; }

        public ICollection<CoinbaseTransaction> Data { get; set; }

        public class PaginationDetails
        {
            [JsonPropertyName("ending_before")]
            public string EndingBefore { get; set; } = string.Empty;

            [JsonPropertyName("starting_after")]
            public string StartingAfter { get; set; } = string.Empty;

            public int Limit { get; set; }

            public string Order { get; set; }

            [JsonPropertyName("previous_uri")]
            public string PreviousUri { get; set; } = string.Empty;

            [JsonPropertyName("next_uri")]
            public string NextUri { get; set; } = string.Empty;
        }
    }
}
