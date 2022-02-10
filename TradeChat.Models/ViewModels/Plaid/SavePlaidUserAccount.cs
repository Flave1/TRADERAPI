using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TradeChat.Data.ViewModels.Plaid
{
    public class SavePlaidUserAccount
    {
        [JsonPropertyName("public_token")]
        public string PublicToken { get; set; }
        public AccountMetaData MetaData { get; set; }

        public class AccountMetaData
        {
            [JsonPropertyName("link_session_id")]
            public string LinkSessionId { get; set; }
            public Institution Institution { get; set; }
            public ICollection<Account> Accounts { get; set; }
        }

        public class Institution
        {
            public string Name { get; set; }
            [JsonPropertyName("institution_id")]
            public string InstitutionId { get; set; }
        }

        public class Account
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Mask { get; set; }
            public string Type { get; set; }
            public string Subtype { get; set; }
            [JsonPropertyName("verification_status")]
            public string VerificationStatus { get; set; }
        }
    }
}
