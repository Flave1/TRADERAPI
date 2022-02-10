using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TradeChat.Models.ViewModels.Exceptions;
using TradeChat.Services.Luno.Models;
using TradeChat.Services.WebRequestHelper;

namespace TradeChat.Services.Luno
{
    public class LunoAuthenticationService : ILunoAuthenticationService
    {
        private readonly LunoOptions config;
        private readonly IWebRequestService webRequestService;

        public LunoAuthenticationService(IOptions<LunoOptions> options, IWebRequestService webRequestService)
        {
            this.config = options.Value;
            this.webRequestService = webRequestService;
        }

        /// <summary>
        /// Validate API Key and Secret
        /// </summary>
        /// <param name="apiKeyId">API Key Id</param>
        /// <param name="apiKeySecret">API Key Secret</param>
        /// <returns></returns>
        public async Task<LunoAuthorizationData> TestApiKey(string apiKeyId, string apiKeySecret)
        {
           

            try
            { 
                string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                                               .GetBytes(apiKeyId + ":" + apiKeySecret));
                var header = new Dictionary<string, string>
                {
                    { "Authorization", $"Basic {encoded}" }
                };
                var result = await webRequestService.BasicGetAsync<GetLunoBalance>(config.BalanceUrl, header);
                if (result != null)
                {
                    return new LunoAuthorizationData
                    {
                        ApiKeyID = apiKeyId,
                        ApiKeySecret = apiKeySecret
                    };
                }
                else
                {
                    throw new AccountNotFoundException("User luno account not found");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create basic authentication header
        /// </summary>
        /// <param name="apiKeyId">API Key Id</param>
        /// <param name="apiKeySecret">API Key Secret</param>
        /// <returns></returns>
        private AuthenticationHeaderValue GetBasicAuthenticationHeader(string apiKeyId, string apiKeySecret)
        {
            byte[] buffer = Encoding.ASCII.GetBytes($"{apiKeyId}:{apiKeySecret}");

            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(buffer));
        }
    }
}
