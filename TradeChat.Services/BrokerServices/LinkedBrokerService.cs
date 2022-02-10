using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeChat.Data.Documents;
using TradeChat.Data.ViewModels;
using TradeChat.Data.ViewModels.Plaid;
using TradeChat.Data.ViewModels.Zabo;
using TradeChat.Models.ViewModels;
using TradeChat.Models.ViewModels.Coinbase;
using TradeChat.Services.Models;
using TradeChat.Services.Plaid;
using TradeChat.Services.Repository;

namespace TradeChat.Services.BrokerServices
{
    public class LinkedBrokerService : ILinkedBrokerService
    {
        private readonly IDocumentRepository<LinkedBrokerDocument> linkedBrokerRepo;
        private readonly IDocumentRepository<UserDocument> userRepo;
        private readonly IDocumentRepository<BrokerDocument> brokerRepo;
        private readonly IPlaidLinkService plaidLinkService;

        public LinkedBrokerService(
            IDocumentRepository<LinkedBrokerDocument> linkedBrokerRepo,
            IDocumentRepository<UserDocument> userRepo,
            IDocumentRepository<BrokerDocument> brokerRepo,
            IPlaidLinkService plaidLinkService
        )
        {
            this.linkedBrokerRepo = linkedBrokerRepo;
            this.userRepo = userRepo;
            this.brokerRepo = brokerRepo;
            this.plaidLinkService = plaidLinkService;
        }

        public async Task<ICollection<BrokerDto>> GetAsync(UserClaimsInfo user)
        {
            var userDoc = await userRepo.FindByIdAsync(user.Id);
            var brokers = await brokerRepo.FilterByAsync(x => userDoc.Brokers.Contains(x.Id));

            return brokers.Select(x => new BrokerDto
            {
                Id = x.Id,
                DisplayName = x.DisplayName,
                Logo = x.Logo,
                Provider = x.Provider,
                Type = x.Type
            }).ToList();
        }

        public async Task LinkAsync(ZaboUserAccount account, UserClaimsInfo user)
        {
            var linkedBrokerDoc = new LinkedBrokerDocument
            {
                UserId = user.Id,
                BrokerId = account.Provider.Name,
                BrokerUserAccountId = account.Id
            };

            var userDoc = await userRepo.FindByIdAsync(user.Id);
            if (userDoc.Brokers == null)
            {
                userDoc.Brokers = new List<string>();
            }

            if (!userDoc.Brokers.Contains(account.Provider.Name))
            {
                userDoc.Brokers.Add(account.Provider.Name);
            }

            await linkedBrokerRepo.InsertOneAsync(linkedBrokerDoc);
            await userRepo.ReplaceOneAsync(userDoc);
        }

        public async Task LinkAsync(GetCoinbaseAuthData account, UserClaimsInfo user)
        {
            var linkedBrokerDoc = new LinkedBrokerDocument
            {
                UserId = user.Id,
                BrokerId = account.Provider,
                BrokerUserAccountId = account.Data.Id.ToString()
            };

            var userDoc = await userRepo.FindByIdAsync(user.Id);
            if (userDoc.Brokers == null)
            {
                userDoc.Brokers = new List<string>();
            }

            if (!userDoc.Brokers.Contains(account.Provider))
            {
                userDoc.Brokers.Add(account.Provider);
            }

            await linkedBrokerRepo.InsertOneAsync(linkedBrokerDoc);
            await userRepo.ReplaceOneAsync(userDoc);
        }

        public async Task LinkAsync(SavePlaidUserAccount account, UserClaimsInfo user)
        {
            var itemId = await plaidLinkService.SaveItemAsync(account.PublicToken, user);
            var brokerId = account.MetaData.Institution.InstitutionId;
            var linkedBrokerDoc = new LinkedBrokerDocument
            {
                UserId = user.Id,
                BrokerId = brokerId,
                BrokerUserAccountId = itemId
            };

            var userDoc = await userRepo.FindByIdAsync(user.Id);
            if (userDoc.Brokers == null)
            {
                userDoc.Brokers = new List<string>();
            }

            if (!userDoc.Brokers.Contains(brokerId))
            {
                userDoc.Brokers.Add(brokerId);
            }

            await linkedBrokerRepo.InsertOneAsync(linkedBrokerDoc);
            await userRepo.ReplaceOneAsync(userDoc);
        }

        public async Task LinkAsync(ProviderKeyDto account, UserClaimsInfo user)
        {
            var linkedBrokerDoc = new LinkedBrokerDocument
            {
                UserId = user.Id,
                BrokerId = account.Provider,
                BrokerUserAccountId = account.Key1,
                Key = account.Key2,
            };

            var userDoc = await userRepo.FindByIdAsync(user.Id);
            if (userDoc.Brokers == null)
            {
                userDoc.Brokers = new List<string>();
            }

            if (!userDoc.Brokers.Contains(account.Provider))
            {
                userDoc.Brokers.Add(account.Provider);
            }

            await linkedBrokerRepo.InsertOneAsync(linkedBrokerDoc);
            await userRepo.ReplaceOneAsync(userDoc);
        }
    }
}
