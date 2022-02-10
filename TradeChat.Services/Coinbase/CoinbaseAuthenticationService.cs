using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TradeChat.Auth.Services;
using TradeChat.Models;
using TradeChat.Models.Options;
using TradeChat.Models.ViewModels.Coinbase;
using TradeChat.Models.ViewModels.Exceptions;
using TradeChat.Services.BrokerServices;
using TradeChat.Services.Coinbase;
using TradeChat.Services.Coinbase.Models;
using TradeChat.Services.Models;
using TradeChat.Services.WebRequestHelper;

namespace TradeChat.Services
{
    public class CoinbaseAuthenticationService : ICoinbaseAuthorizationService
    {
        private readonly CoinbaseOptions config;
        private readonly EncryptionOptions encryptionConfig;
        private readonly ApplicationOptions applicationSettings;
        private readonly IOAuthStateManager AuthStateManager;
        private readonly ILinkedBrokerService linkedBrokerService;
        private readonly IWebRequestService webRequestService;
        private readonly IAccessTokenService accessTokenService;


        public CoinbaseAuthenticationService(
            IOAuthStateManager authStateManager,
            ILinkedBrokerService linkedBrokerService,
            IWebRequestService webRequestService,
            IAccessTokenService accessTokenService,
            IOptions<CoinbaseOptions> options,
            IOptions<ApplicationOptions> appConfigOptions,
            IOptions<EncryptionOptions> encryptionOptions)
        {
            this.AuthStateManager = authStateManager;
            this.linkedBrokerService = linkedBrokerService;
            this.webRequestService = webRequestService;
            this.accessTokenService = accessTokenService;
            config = options.Value;
            applicationSettings = appConfigOptions.Value;
            encryptionConfig = encryptionOptions.Value;
        }

        /// <summary>
        /// Get Coinbase authorization Url
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAuthorizationUrl()
        {
            var randState = await AuthStateManager.GenerateState();
            var url = $"{config.AuthorizeUrl}?response_type=code&client_id={config.ClientId}&redirect_uri={config.CallbackUrl}&scope={config.Scopes}&state={randState.State}";
            return url;
        }

        /// <summary>
        /// Exchange authorization code for access tokens
        /// </summary>
        /// <param name="code">Authorization code from initial oauth request</param>
        /// <returns></returns>
        public async Task<CoinbaseAuthorizationData> GetTokens(string code)
        {

            try
            {
                var input = new GetCoinbaseInput
                {
                    GrantType = "authorization_code",
                    code = code,
                    ClientId = config.ClientId,
                    ClientSecret = config.ClientSecret,
                    RedirectUri = config.CallbackUrl
                };
                var result = await webRequestService.PostAsync<CoinbaseAuthorizationData, GetCoinbaseInput>(config.TokenUrl, input, null);
                if (result != null)
                {
                    return result;
                }
                throw new AccountNotFoundException("User broker account not found");
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task SaveUserCoinBaseData(string accessToken, UserClaimsInfo user)
        {

            var header = new Dictionary<string, string> { 
                { "Authorization", $"Bearer {accessToken}" },
                { "CB-VERSION", "2018-05-21" }
            };

            var result = await webRequestService.GetAsync<GetCoinbaseAuthData>(config.UserProfile, header);
            if (result != null)
            {
                if (result == null)
                {
                    throw new UnableToSaveAuthDataException("Could not save user coinbase data");
                }
                result.Provider = "coinbase";
                await SaveAccessTokenInWorker(accessToken, result.Data.Id, user.Id);
                await linkedBrokerService.LinkAsync(result, user);
            }
        }

        private async Task SaveAccessTokenInWorker(string coinbaseAccessToken, string coinbaseUserId, string userId)
        {
            var workerUrl = $"{applicationSettings.WorkerApiUrl}/api/coinbase/save";
            var input = new SaveCoinbaseLinkItem
            {
                AccessToken = coinbaseAccessToken,
                UserId = userId,
                CoinbaseUserId = coinbaseUserId,
                BrokerId = "coinbase"
            };

            var accessToken = await accessTokenService.GetAsync();
            if(accessToken != null)
            {
                var header = new Dictionary<string, string>
                 {
                    { "Authorization", $"Bearer {accessToken}"}
                 };
                await webRequestService.PostAsync(workerUrl, input, header);
            }
            
        }
    }
}
