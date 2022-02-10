using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradeChat.Auth.Services;
using TradeChat.Data.MessageQueueModels;
using TradeChat.Triggers.Services.WebRequestHelper;

namespace TradeChat.Triggers.Functions
{
    public class GetPlaidInvestment
    {
        private readonly IWebRequestService webRequestService;
        private readonly IAccessTokenService accessTokenService;

        public GetPlaidInvestment(IWebRequestService webRequestService, IAccessTokenService accessTokenService)
        {
            this.webRequestService = webRequestService;
            this.accessTokenService = accessTokenService;
        }

        [FunctionName("GetPlaidInvestment")]
        public async Task Run([ServiceBusTrigger("%PlaidInvestmentQueue%", Connection = "ServiceBusConnectionString")]
            Message queueMessage,
            ILogger log)
        {
            string payload = Encoding.UTF8.GetString(queueMessage.Body);
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {payload}");
            var data = JsonConvert.DeserializeObject<PlaidInvestmentQueueMessage>(payload);
            var url = $"{Environment.GetEnvironmentVariable("WorkerApiUrl")}/investment/{data.Id}";

            var accessToken = await accessTokenService.GetAsync();
            var header = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {accessToken}" }
            };

            await webRequestService.GetAsync(url, header);
        }
    }
}
