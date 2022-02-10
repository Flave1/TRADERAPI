using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TradeChat.Models.ViewModels.Exceptions;
using TradeChat.Services.Kraken.Models;
using TradeChat.Services.WebRequestHelper;

namespace TradeChat.Services.Kraken
{
    public class KrakenAuthenticationService : IKrakenAuthenticationService
    {
        private readonly KrakenOptions config;
        private readonly IWebRequestService webRequestService;
        public KrakenAuthenticationService(IOptions<KrakenOptions> options, IWebRequestService webRequestService)
        {
            this.config = options.Value;
            this.webRequestService = webRequestService;
        }

        /// <summary>
        /// Validate Api Key and Secret
        /// </summary>
        /// <param name="key">API key</param>
        /// <param name="privateKey">API private key</param>
        /// <param name="otp">OTP for two factor authentication</param>
        /// <returns></returns>
        public async Task<KrakenAuthorizationData> TestApiKey(string key, string privateKey, string otp)
        {

            Int64 nonce = DateTime.UtcNow.Ticks;
            string parameters = $"nonce={nonce}" +
                (string.IsNullOrEmpty(otp) ? "" : $"&otp={otp}");

            privateKey = privateKey.Replace(" ", "+");
            string signature = GetKrakenSignature(config.BalancePath, parameters, nonce, privateKey);
            var keyVals = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("nonce",nonce.ToString())
            };
            if (!string.IsNullOrEmpty(otp))
            {
                keyVals.Add(new KeyValuePair<string, string>("otp", otp));
            }
            FormUrlEncodedContent content = new FormUrlEncodedContent(keyVals);
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, $"{config.BaseUrl}{config.BalancePath}");
            req.Content = content;
            req.Headers.Add("API-KEY", key);
            req.Headers.Add("API-SIGN", signature);
            HttpClient client = new HttpClient();
            HttpResponseMessage res = await client.SendAsync(req);
            if (res.IsSuccessStatusCode)
            {
                var response = JsonConvert.DeserializeObject<GetKrakenBalance>(await res.Content.ReadAsStringAsync());
                if (response.Error.Count() > 0)
                {
                    throw new ArgumentException(response.Error.FirstOrDefault());
                }
                else
                {
                    return new KrakenAuthorizationData
                    {
                        Key = key,
                        PrivateKey = privateKey
                    };
                }
            }
            throw new AccountNotFoundException("User kraken account not found"); ;
        }

        /// <summary>
        /// Generate request signature
        /// </summary>
        /// <param name="path">request path</param>
        /// <param name="parameters">query parameters</param>
        /// <param name="nonce">unique incremental number</param>
        /// <param name="privateKey">API private key</param>
        /// <returns></returns>
        private string GetKrakenSignature(string path, string parameters, long nonce, string privateKey)
        {
            var np = nonce + parameters;
            byte[] nonceParamsBytes;
            using (var sha = SHA256.Create())
                nonceParamsBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(np));
            var pathBytes = Encoding.UTF8.GetBytes(path);
            var allBytes = pathBytes.Concat(nonceParamsBytes).ToArray();
            var encryptor = new HMACSHA512(Convert.FromBase64String(privateKey));
            var sign = encryptor.ComputeHash(allBytes);
            var ret = Convert.ToBase64String(sign);
            return ret;
        }

    }
}