using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using TradeChat.Data.Documents;
using TradeChat.Data.Entities;
using TradeChat.Services.Plaid.Models;
using TradeChat.Services.Repository;
using TradeChat.Services.WebRequestHelper;

namespace TradeChat.Services.Plaid
{
    public class PlaidAccessTokenService : IPlaidAccessTokenService
    {
        private readonly IDocumentRepository<PlaidLinkDocument> documentRepository;
        private readonly IEntityRepository<PlaidEntity> entityRepository;

        public PlaidAccessTokenService(
            IDocumentRepository<PlaidLinkDocument> documentRepository,
            IEntityRepository<PlaidEntity> entityRepository,
            IWebRequestService webRequestService,
            IOptions<PlaidOptions> options
        )
        {
            this.documentRepository = documentRepository;
            this.entityRepository = entityRepository;
        }

        public async Task SaveAsync(SavePlaidLinkItem item)
        {
            //get linked item by id from the document db
            var linkedDoc = await documentRepository.FindByIdAsync(item.DocumentId);
            if (linkedDoc == null)
            {
                return;
            }

            //encrypt access token and create a new plaid entity
            var entity = new PlaidEntity
            {
                AccessToken = item.AccessToken,
                ItemId = linkedDoc.ItemId,
                UserId = linkedDoc.UserId
            };

            await entityRepository.AddAsync(entity);
        }
    }
}
