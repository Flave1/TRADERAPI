using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TradeChat.Models.ViewModels.Exceptions;
using TradeChat.Services.Liquid.Models;
using TradeChat.Services.WebRequestHelper;

namespace TradeChat.Services.Liquid
{
    public class LiquidAuthenticationService : ILiquidAuthenticationService
    {
        private readonly LiquidOptions config;
        private readonly IWebRequestService webRequestService;
        public LiquidAuthenticationService(IOptions<LiquidOptions> options, IWebRequestService webRequestService)
        {
            this.config = options.Value;
            this.webRequestService = webRequestService;
        }

        /// <summary>
        /// Validate API key and secret
        /// </summary>
        /// <param name="apiTokenId">API token id</param>
        /// <param name="apiSecret">API secret</param>
        /// <returns></returns>
        public async Task<LiquidAuthorizationData> TestApiKey(string apiTokenId, string apiSecret)
        {
            Int64 nonce = DateTime.UtcNow.Ticks;
            string signature = GetLiquidSignature(apiTokenId, apiSecret, config.BankAccountsPath, nonce);


            var headers = new Dictionary<string, string>
                 {
                    { "X-Quoine-API-Version", config.ApiVersion },
                    {"X-Quoine-Auth", signature }
                };
            var result = await webRequestService.GetAsync<GetLiquidModels>($"{config.BaseUrl}{config.BankAccountsPath}", headers);
            if (result != null)
            {
                return new LiquidAuthorizationData
                {
                    ApiTokenId = apiTokenId,
                    ApiSecret = apiSecret,
                };
            }
            throw new AccountNotFoundException("User Liquid account not found");
        }

        /// <summary>
        /// Generate request signature
        /// </summary>
        /// <param name="tokenId">Token Id</param>
        /// <param name="apiSecret">API Secret</param>
        /// <param name="path">Request path</param>
        /// <param name="nonce">Unique unsigned integer</param>
        /// <returns></returns>
        private string GetLiquidSignature(string tokenId, string apiSecret, string path, Int64 nonce)
        {
            apiSecret = apiSecret.Replace(" ", "+");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiSecret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var jwtHeader = new JwtHeader(signingCredentials);
            var jwtPayload = new JwtPayload(new List<Claim> {
                new Claim("path",path),
                new Claim("nonce",nonce.ToString()),
                new Claim("token_id",tokenId)
            });
            var securityToken = new JwtSecurityToken(jwtHeader, jwtPayload);
            var JWT = new JwtSecurityTokenHandler();
            var ret = JWT.WriteToken(securityToken);
            return ret;
        }
    }
}
