using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradeChat.Data.Documents;
using TradeChat.Data.Enums;
using TradeChat.Data.ViewModels;
using TradeChat.Services.Repository;
using TradeChat.Services.WebRequestHelper;
using TradeChat.Services.Zabo.Models;

namespace TradeChat.Services.Zabo
{
    public class GetZaboBrokerService : IGetZaboBrokerService
    {
        private readonly ZaboConfigOptions config;
        private readonly IZaboAuthenticationService authenticationService;
        private readonly IWebRequestService webRequestService;
        private readonly IDocumentRepository<BrokerDocument> repository;

        public GetZaboBrokerService(
            IOptions<ZaboConfigOptions> options,
            IZaboAuthenticationService authenticationService,
            IWebRequestService webRequestService,
            IDocumentRepository<BrokerDocument> repository
        )
        {
            this.config = options.Value;
            this.authenticationService = authenticationService;
            this.webRequestService = webRequestService;
            this.repository = repository;
        }

        public async Task LoadAsync()
        {
            var brokerList = new List<BrokerDto>();

            string url = $"{config.BaseUrl}/providers?limit=100";
            var headers = authenticationService.Authenticate(url);
            var result = await webRequestService.GetAsync<GetBrokerResponse>(url, headers);
            if (result.Data != null)
            {
                var newbrokers = new List<BrokerDocument>();
                Array.ForEach(result.Data, async (x) =>
                {
                    var broker = new BrokerDocument
                    {
                        DisplayName = x.Displayname,
                        Id = x.Name,
                        Logo = x.Logo,
                        Provider = "zabo",
                        Type = BrokerType.Crypto
                    };

                    var savedBroker = await repository.FindByIdAsync(broker.Id);
                    if (savedBroker != null)
                    {
                        await repository.ReplaceOneAsync(broker);
                    }
                    else
                    {
                        newbrokers.Add(broker);
                    }
                });

                if (newbrokers.Count > 0)
                {
                    await repository.InsertManyAsync(newbrokers);
                }
            }
        }
    }
}
