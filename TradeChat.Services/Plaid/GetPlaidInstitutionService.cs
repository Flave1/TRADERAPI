using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradeChat.Data.Documents;
using TradeChat.Data.Enums;
using TradeChat.Data.ViewModels;
using TradeChat.Services.Plaid.Models;
using TradeChat.Services.Repository;
using TradeChat.Services.WebRequestHelper;

namespace TradeChat.Services.Plaid
{
    public class GetPlaidInstitutionService : IGetPlaidInstitutionService
    {
        private readonly PlaidOptions config;
        private readonly IWebRequestService webRequestService;
        private readonly IDocumentRepository<BrokerDocument> repository;

        public GetPlaidInstitutionService(
            IOptions<PlaidOptions> options,
            IWebRequestService webRequestService,
            IDocumentRepository<BrokerDocument> repository
        )
        {
            config = options.Value;
            this.webRequestService = webRequestService;
            this.repository = repository;
        }

        public async Task LoadAsync()
        {
            string url = $"{config.BaseUrl}/institutions/get";
            var headers = new Dictionary<string, string>
            {
                { "PLAID-CLIENT-ID", config.ClientId },
                { "PLAID-SECRET", config.Secret }
            };

            var input = new GetInstitutionInput
            {
                Count = 50,
                CountryCodes = new[] { "CA" },
                Offset = 0
            };

            var brokerList = new List<BrokerDto>();

            int total = 50;
            int current = 0;

            do
            {
                var result = await webRequestService.PostAsync<GetInstitutionOutput, GetInstitutionInput>(url, input, headers);
                if (result.Institutions != null)
                {
                    Array.ForEach(result.Institutions, async (x) =>
                    {
                        var broker = new BrokerDocument
                        {
                            DisplayName = x.Name,
                            Id = x.InstitutionId,
                            Logo = x.Logo,
                            Provider = "plaid",
                            Type = BrokerType.Stock
                        };

                        var savedBroker = await repository.FindByIdAsync(broker.Id);
                        if (savedBroker != null)
                        {
                            await repository.ReplaceOneAsync(broker);
                        }
                        else
                        {
                            await repository.InsertOneAsync(broker);
                        }
                    });

                    total = result.Total;
                    current += result.Institutions.Length;
                    input.Offset = current;
                }
            }
            while (current < total);
        }
    }
}
