using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TradeChat.Services.Gemini.Models
{
    class PostGeminiAuthData
    {
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("redirect_uri")]
        public string RedirectUri { get; set; }
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }
    }
}
