using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradeChat.Auth.Services;
using TradeChat.Data.Documents;
using TradeChat.Models.Options;
using TradeChat.Services.Models;
using TradeChat.Services.Plaid.Models;
using TradeChat.Services.Repository;
using TradeChat.Services.WebRequestHelper;

namespace TradeChat.Services.Plaid
{
    public class PlaidLinkService : IPlaidLinkService
    {
        private readonly IDocumentRepository<PlaidLinkDocument> linkRepo;
        private readonly IWebRequestService webRequestService;
        private readonly IAccessTokenService accessTokenService;
        private readonly PlaidOptions plaidConfig;
        private readonly ApplicationOptions appConfig;
        private readonly EncryptionOptions encryptionConfig;


        public PlaidLinkService(
            IDocumentRepository<PlaidLinkDocument> linkRepo,
            IWebRequestService webRequestService,
            IAccessTokenService accessTokenService,
            IOptions<PlaidOptions> plaidOptions,
            IOptions<ApplicationOptions> applicationOptions,
            IOptions<EncryptionOptions> encryptionConfigOptions
        )
        {
            this.webRequestService = webRequestService;
            this.accessTokenService = accessTokenService;
            this.linkRepo = linkRepo;
            plaidConfig = plaidOptions.Value;
            appConfig = applicationOptions.Value;
            encryptionConfig = encryptionConfigOptions.Value;
        }

        public async Task<string> GetLinkTokenAsync(UserClaimsInfo user)
        {
            var webhookUrl = plaidConfig.WebhookUrl;
            var input = new CreateLinkInput
            {
                ClientId = plaidConfig.ClientId,
                ClientName = plaidConfig.ClientName,
                CountryCodes = new[] { "CA", "US" },
                Language = "en",
                Secret = plaidConfig.Secret,
                Products = new[] { "investments" },
                Webhook = webhookUrl,
                User = new CreateLinkInput.ClientUser
                {
                    ClientUserId = user.Id
                },
            };

            string url = $"{plaidConfig.BaseUrl}/link/token/create";
            var result = await webRequestService.PostAsync<CreateLinkOutput, CreateLinkInput>(url, input);
            return result.LinkToken;
        }

        public async Task<string> SaveItemAsync(string publicToken, UserClaimsInfo user)
        {
            var getAccessTokenInput = new GetPlaidAccessTokenInput
            {
                ClientId = plaidConfig.ClientId,
                Secret = plaidConfig.Secret,
                PublicToken = publicToken
            };

            var url = $"{plaidConfig.BaseUrl}/item/public_token/exchange";
            var result = await webRequestService.PostAsync<GetPlaidAccessTokenOutput, GetPlaidAccessTokenInput>(url, getAccessTokenInput);

            var linkedDoc = new PlaidLinkDocument
            {
                PublicToken = publicToken,
                ItemId = result.ItemId,
                RequestId = result.RequestId,
                UserId = user.Id
            };

            await linkRepo.InsertOneAsync(linkedDoc);
            await SaveAccessTokenInWorker(linkedDoc.Id, result.AccessToken);
            return result.ItemId;
        }

        private async Task SaveAccessTokenInWorker(string documentId, string plaidAccessToken)
        {
            var workerUrl = $"{appConfig.WorkerApiUrl}/api/plaid/save";
            var input = new SavePlaidLinkItem
            {
                AccessToken = Encryption.AESEncrypt(encryptionConfig.BrokerAccountKey, plaidAccessToken),
                DocumentId = documentId
            };

            var accessToken = accessTokenService.GetAsync();
            var header = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {accessToken}"}
            };

            await webRequestService.PostAsync(workerUrl, input, header);
        }
    }
}
