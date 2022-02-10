using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TradeChat.Services.Coinbase;
using TradeChat.Services.Coinbase.Models;
using TradeChat.Services.Gemini.Models;
using TradeChat.Services.WebRequestHelper;

namespace TradeChat.Services.Gemini
{
    public class GeminiAuthenticationService : IGeminiAuthenticationService
    {
        private readonly IOAuthStateManager authStateManager;
        private readonly GeminiConfigOptions config;
        private readonly IWebRequestService webRequestService;

        public GeminiAuthenticationService(IOptions<GeminiConfigOptions> config, IOAuthStateManager authStateManager, IWebRequestService webRequestService)
        {
            this.config = config.Value;
            this.authStateManager = authStateManager;
            this.webRequestService = webRequestService;
        }

        /// <summary>
        /// Get Gemini authorization url
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAuthorizationUrl()
        {
            StateModel state = await authStateManager.GenerateState();
            return $"{config.AuthorizeUrl}?client_id={config.ClientId}&response_type=code&redirect_uri={config.RedirectUrl}&state={state.State}&scope={config.Scopes}";
        }

        /// <summary>
        /// Exchange authorization code for access tokens
        /// </summary>
        /// <param name="code">Authorization code from initial oauth request</param>
        /// <returns></returns>
        public async Task<GetGeminiAuthorizationData> GetTokens(string code)
        {
            try
            {
                var input = new PostGeminiAuthData
                {
                    ClientId = config.ClientId,
                    ClientSecret = config.ClientSecret,
                    Code = code,
                    RedirectUri = config.RedirectUrl,
                    GrantType = "authorization_code"
                };
                var res = await webRequestService.PostAsync<GetGeminiAuthorizationData, PostGeminiAuthData>(config.TokenUrl, input);
                return res;
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
