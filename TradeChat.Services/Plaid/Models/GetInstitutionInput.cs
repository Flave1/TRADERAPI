using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TradeChat.Services.Plaid.Models
{
    public class GetInstitutionInput
    {
        public int Count { get; set; }
        public int Offset { get; set; }
        [JsonPropertyName("country_codes")]
        public ICollection<string> CountryCodes { get; set; }

        public GetInstitutionInputOptions Options;

        public GetInstitutionInput()
        {
            CountryCodes = new List<string>() { "CA" };
            Options = new GetInstitutionInputOptions();
        }

        public class GetInstitutionInputOptions
        {
            public string[] Products = new[] { "investments" };

            [JsonPropertyName("include_optional_metadata")]
            public bool IncludeOptionalMetaData = true;
        }
    }

}
