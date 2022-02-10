using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradeChat.Services.Gemini.Models;

namespace TradeChat.Services.Gemini
{
    public interface IGeminiAuthenticationService
    {
        Task<string> GetAuthorizationUrl();
        Task<GetGeminiAuthorizationData> GetTokens(string code);
    }
}
