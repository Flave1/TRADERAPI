using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TradeChat.Models.ViewModels.Exceptions;
using TradeChat.Services.Binance.Models;
using TradeChat.Services.Coinbase;
using TradeChat.Services.WebRequestHelper;

namespace TradeChat.Services.Binance
{

    public class BinanceAuthenticationService : IBinanceAuthenticationService
    {
        private readonly IWebRequestService webRequestService;
        private readonly BinanceOptions config;

        private IOAuthStateManager stateManager;

        public BinanceAuthenticationService(IOAuthStateManager stateManager, IWebRequestService webRequestService, IOptions<BinanceOptions> options)
        {
            this.stateManager = stateManager;
            this.webRequestService = webRequestService;
            this.config = options.Value;
        }

        /// <summary>
        /// Get Binance authorization url
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAuthorizationUrl()
        {
            var randState = await stateManager.GenerateState();
            return $"{config.AuthorizeUrl}?response_type=code&client_id={config.ClientId}" +
                $"&redirect_uri={HttpUtility.UrlEncode(config.CallbackUrl)}&state={randState.State}&scope={config.Scopes}";
        }

        /// <summary>
        /// Exchange Authorization code for access tokens
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<BinanceOAuthAuthorizationData> GetTokens(string code)
        {
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, config.TokensUrl);
            req.Content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("grant_type","authorization_code"),
                new KeyValuePair<string, string>("code",code),
                new KeyValuePair<string, string>("client_id",config.ClientId),
                new KeyValuePair<string, string>("client_secret",config.ClientSecret),
                new KeyValuePair<string, string>("redirect_url",HttpUtility.UrlEncode(config.CallbackUrl)),
            });
            HttpClient client = new HttpClient();
            HttpResponseMessage res = await client.SendAsync(req);
            var result = await webRequestService.PostAsync<BinanceOAuthAuthorizationData, HttpRequestMessage>(config.TokensUrl, null);
            if (result != null)
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Validate API Key and Secret
        /// </summary>
        /// <param name="apiKey">API Key</param>
        /// <param name="apiSecret">API Secret</param>
        /// <returns></returns>
        public async Task<BinanceAuthorizationData> TestApiKey(string apiKey, string apiSecret)
        {
            var header = new Dictionary<string, string> { { "X-MBX-APIKEY", apiKey } };
            var result = new BinanceAuthorizationData();
            var timestamp = GetUnixTimeStamp();
            var requestUrl = ReturnRequestUri(apiSecret, timestamp); 
           
            result = await webRequestService.GetAsync<BinanceAuthorizationData>(requestUrl, header);
            if(result.Code == -1021) // Status code for Timestamp for this request is outside of the recvWindow.
            {
                var postTimestamp = GetPostUnixTimeStamp();
                var newRequestUrl = ReturnRequestUri(apiSecret, postTimestamp);  
                result = await webRequestService.GetAsync<BinanceAuthorizationData>(newRequestUrl, header);
            }
            if (result.Code < 0)
            {
                throw new UnableToSaveAuthDataException(result.Msg);
            }
            return new BinanceAuthorizationData
            {
                ApiKey = apiKey,
                ApiSecret = apiSecret,
            };
            throw new AccountNotFoundException("User broker account not found");
        }


        private string ReturnRequestUri(string secret, string nonce)
        {
            string query = $"timestamp={nonce}";
            string signature = GenerateSignature(secret, query);
            return $"{config.AccountStatusUrl}?{query}&signature={signature}";
        }

        /// <summary>
        /// Convert DateTime to Unix 
        /// </summary>
        /// <param name="baseDateTime"></param>
        /// <returns></returns>
        private  string GetUnixTimeStamp()
        {
            var dtOffset = new DateTimeOffset(DateTime.UtcNow);
            return dtOffset.ToUnixTimeMilliseconds().ToString();
        }

        private string GetPostUnixTimeStamp()
        { 
            var dtOffset = new DateTimeOffset(DateTime.UtcNow.AddMilliseconds(5000));
            return dtOffset.ToUnixTimeMilliseconds().ToString();
        }

        /// <summary>
        /// Generate request signature with API secret
        /// </summary>
        /// <param name="apiSecret">API Secret</param>
        /// <param name="message">Request query</param>
        /// <returns></returns>
        private static string GenerateSignature(string apiSecret, string message)
        {
            var key = Encoding.UTF8.GetBytes(apiSecret);
            string stringHash;
            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
                stringHash = BitConverter.ToString(hash).Replace("-", "");
            }
            return stringHash;
        }
    }
}
