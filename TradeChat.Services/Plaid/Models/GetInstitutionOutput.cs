using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models
{
    internal class GetInstitutionOutput
    {
        public int Total { get; set; }

        [JsonPropertyName("request_id")]
        public string RequestId { get; set; }
        public InstitutionItem[] Institutions { get; set; }
    }

}
