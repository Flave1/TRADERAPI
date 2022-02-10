using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using TradeChat.Models.Options;
using TradeChat.Models.ViewModels;
using TradeChat.Models.ViewModels.Exceptions;
using TradeChat.Services;
using TradeChat.Services.Binance;
using TradeChat.Services.Binance.Models;
using TradeChat.Services.BrokerServices;
using TradeChat.Services.Coinbase;
using TradeChat.Services.Gemini;
using TradeChat.Services.Ecrypt;
using TradeChat.Services.Kraken;
using TradeChat.Services.Liquid;
using TradeChat.Services.Liquid.Models;
using TradeChat.Services.Luno;
using TradeChat.Services.Luno.Models;
using TradeChat.Services.UserServices;

namespace TradeChat.API.Controllers
{
    [EncryptedStream]
    [Controller, Route("api/integrate/broker")]
    public class BrokerIntegrationController : Controller
    {
        private readonly IBinanceAuthenticationService binanceAuthService;
        private readonly ICoinbaseAuthorizationService coinbaseAuthService;
        private readonly ILunoAuthenticationService lunoAuthService;
        private readonly IKrakenAuthenticationService krakenAuthService;
        private readonly ILiquidAuthenticationService liquidAuthService;
        private readonly ILinkedBrokerService linkedBrokerService;
        private readonly IRetrieveUserService retrieveUserService;
        private readonly IOAuthStateManager oauthStateManager;
        private readonly IGeminiAuthenticationService geminiAuthService;

        public BrokerIntegrationController(
            ICoinbaseAuthorizationService coinbaseAuthService,
            IRetrieveUserService retrieveUserService,
            IOAuthStateManager oauthStateManager,
            IBinanceAuthenticationService binanceAuthService,
            ILunoAuthenticationService lunoAuthService,
            IKrakenAuthenticationService krakenAuthService,
            ILiquidAuthenticationService liquidAuthService,
            ILinkedBrokerService linkedBrokerService, IGeminiAuthenticationService geminiAuthService)
        {
            this.coinbaseAuthService = coinbaseAuthService;
            this.oauthStateManager = oauthStateManager;
            this.retrieveUserService = retrieveUserService;
            this.binanceAuthService = binanceAuthService;
            this.lunoAuthService = lunoAuthService;
            this.krakenAuthService = krakenAuthService;
            this.liquidAuthService = liquidAuthService;
            this.linkedBrokerService = linkedBrokerService;
            this.geminiAuthService = geminiAuthService;
        }

        /// <summary>
        /// Gets Coinbase OAuth Endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet("coinbase/oauth")]
        [Description("Returns Coinbase OAuth Endpoint.")]
        [ProducesResponseType(typeof(APIResponse<string>), StatusCodes.Status200OK)]
        [AllowUncryptedStream]
        public async Task<IActionResult> CoinbaseAuthentication()
        {
            var authUrl = await coinbaseAuthService.GetAuthorizationUrl();
            return Ok(new { result = authUrl, });
        }

        /// <summary>
        /// Coinbase OAuth callback endpoint
        /// </summary>
        /// <param name = "request" > Authorization Code initial request</param>  
        /// <returns></returns>
        [HttpPost("coinbase/call-back")]
        public async Task<IActionResult> CoinbaseAuthentication([FromBody] ApikeyRequest request)
        {
            try
            { 
                var user = await retrieveUserService.GetUserClaimsInfo(User);
                await oauthStateManager.ValidateState(request.State);
                var authData = await coinbaseAuthService.GetTokens(request.Code);
                await coinbaseAuthService.SaveUserCoinBaseData(authData.AccessToken, user);
                return Ok(new { result = "success", });
            }
            catch (UnableToSaveAuthDataException ex)
            {
                return StatusCode(400, ex);
            }
            catch (UnAuthorizedBrokerAccessException ex)
            {
                return StatusCode(401, ex);
            }
            catch (AccountNotFoundException ex)
            {
                return StatusCode(404, ex);
            }
            catch (InvalidStateException ex)
            {
                return StatusCode(400, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Binance API Key authentication endpoint
        /// </summary>
        /// <param name="request">API Keyand Api Secret</param> 
        ///// <returns></returns>
        [HttpPost("binance/api-key")]
        public async Task<IActionResult> BinanceAuthentication([FromBody] ApikeyRequest request)
        {
            try
            {

                var user = await retrieveUserService.GetUserClaimsInfo(User);
                BinanceAuthorizationData authData = await binanceAuthService.TestApiKey(request.ApiKey, request.ApiSecret);
                await linkedBrokerService.LinkAsync(new ProviderKeyDto { Key1 = authData.ApiKey, Key2 = authData.ApiSecret, Provider = "Binance" }, user);
                return Ok(new { result = true });
            }
            catch (UnableToSaveAuthDataException ex)
            {
                return StatusCode(400, ex);
            }
            catch (AccountNotFoundException ex)
            {
                return StatusCode(400, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("binance/oauth")]
        [AllowUncryptedStream]
        public async Task<IActionResult> BinanceOAuthAuthentication()
        {
            string redirectUrl = await binanceAuthService.GetAuthorizationUrl();
            return Ok(new { result = redirectUrl });
        }

        [HttpPost("binance/oauth/call-back")]
        public async Task<IActionResult> BinanceOAuthAuthentication([FromBody] ApikeyRequest request)
        {
            try
            { 
                await oauthStateManager.ValidateState(request.State);
                BinanceOAuthAuthorizationData authData = await binanceAuthService.GetTokens(request.Code);
                return Ok(new { result = "success" });
            }
            catch (InvalidStateException ex)
            {
                return StatusCode(400, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Luno API Key authentication endpoint
        /// </summary>
        /// <param name="request">API Key ID and API Key Secret</param> 
        /// <returns></returns>
        [HttpPost("luno/api-key")]
        public async Task<IActionResult> LunoAuthentication([FromBody] ApikeyRequest request)
        {
            try
            { 

                var user = await retrieveUserService.GetUserClaimsInfo(User);
                LunoAuthorizationData authData = await lunoAuthService.TestApiKey(request.ApiKey, request.ApiSecret);
                await linkedBrokerService.LinkAsync(new ProviderKeyDto { Key1 = authData.ApiKeyID, Key2 = authData.ApiKeySecret, Provider = "luno" }, user);
                return Ok(new { result = true });
            }
            catch (AccountNotFoundException ex)
            {
                return StatusCode(400, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Kraken API Key authentication endpoint
        /// </summary>
        /// <param name="request">API Key || API Private Key || OTP for two factor authentication</param> 
        /// <returns></returns>
        [HttpPost("kraken/api-key")]
        public async Task<IActionResult> KrakenAuthentication([FromBody] ApikeyRequest request)
        {
            try
            { 

                var user = await retrieveUserService.GetUserClaimsInfo(User);
                var authData = await krakenAuthService.TestApiKey(request.ApiKey, request.ApiSecret, request.Otp > 0 ? request.Otp.ToString() : string.Empty);
                await linkedBrokerService.LinkAsync(new ProviderKeyDto { Key1 = authData.Key, Key2 = authData.PrivateKey, Provider = "kraken" }, user);
                return Ok(new { result = true });
            }
            catch (AccountNotFoundException ex)
            {
                return StatusCode(404, ex);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        /// <summary>
        /// Liquid API Key authentication endpoint
        /// </summary>
        /// <param name="request">API Token Id and API Secret</param> 
        /// <returns></returns>
        [HttpPost("liquid")]
        public async Task<IActionResult> LiquidAuthentication([FromBody] ApikeyRequest request)
        {
            try
            { 
                var user = await retrieveUserService.GetUserClaimsInfo(User);
                LiquidAuthorizationData authData = await liquidAuthService.TestApiKey(request.ApiKey, request.ApiSecret);
                if (authData != null)
                {
                    await linkedBrokerService.LinkAsync(new ProviderKeyDto { Key1 = authData.ApiSecret, Key2 = authData.ApiTokenId, Provider = "kraken" }, user);
                    return Ok(new { result = true });
                }
                return Ok(new { result = false });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }


    }
}