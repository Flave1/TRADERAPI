using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using TradeChat.Models;
using TradeChat.Models.Options;

namespace TradeChat.Auth.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly HttpClient client;
        private readonly AzureADOptions azureADConfig;

        public AccessTokenService(IHttpClientFactory clientFactory, IOptions<AzureADOptions> options)
        {
            client = clientFactory.CreateClient();
            azureADConfig = options.Value;
        }

        public async Task<string> GetAsync()
        {
            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = $"https://login.microsoftonline.com/{azureADConfig.Domain}/oauth2/v2.0/token",
                ClientId = azureADConfig.ClientId,
                ClientSecret = azureADConfig.Secret,
                Scope = $"https://{azureADConfig.Domain}/{azureADConfig.ClientId}/.default"
            });

            return response?.AccessToken;
        }
    }
}
