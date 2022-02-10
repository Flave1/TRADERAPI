using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TradeChat.Services.Gemini.Models
{
    public class GeminiConfigOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizeUrl { get; set; }
        public string TokenUrl { get; set; }
        public string Scopes { get; set; }
        public string RedirectUrl { get; set; }
    }
}
