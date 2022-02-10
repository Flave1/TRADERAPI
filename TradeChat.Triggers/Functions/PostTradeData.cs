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
using TradeChat.Data.ViewModels;
using TradeChat.Triggers.Services.WebRequestHelper;

namespace TradeChat.Triggers.Functions
{
    public class PostTradeData
    {
        private readonly IWebRequestService webRequestService;
        private readonly IAccessTokenService accessTokenService;

        public PostTradeData(IWebRequestService webRequestService, IAccessTokenService accessTokenService)
        {
            this.webRequestService = webRequestService;
            this.accessTokenService = accessTokenService;
        }

        [FunctionName("PostTradeData")]
        public async Task Run([ServiceBusTrigger("%PostTradeQueue%", Connection = "ServiceBusConnectionString")]
            Message queueMessage,
            ILogger log)
        {
            string payload = Encoding.UTF8.GetString(queueMessage.Body);
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {payload}");
            var data = JsonConvert.DeserializeObject<PostTradeQueueMessage>(payload);

            var trade = new TradeDto
            {
                Type = data.Type,
                SecurityType = data.SecurityType,
                Amount = data.Amount,
                BrokerId = data.BrokerId,
                Currency = data.Currency,
                Date = data.Date,
                DestinationItem = data.DestinationItem,
                Fees = data.Fees,
                Price = data.Price,
                Quantity = data.Quantity,
                SourceItem = data.SourceItem,
                UserId = data.UserId
            };

            var url = $"{Environment.GetEnvironmentVariable("GatewayApiUrl")}/trade";

            var accessToken = await accessTokenService.GetAsync();
            var header = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {accessToken}" }
            };

            await webRequestService.PostAsync(url, trade, header);
        }
    }
}
