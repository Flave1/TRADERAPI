using Microsoft.Graph;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TradeChat.Data.Documents;
using TradeChat.Services.Models;
using TradeChat.Services.Repository;

namespace TradeChat.Services.UserServices
{
    public class RetrieveUserService : IRetrieveUserService
    {
        private readonly IDocumentRepository<UserDocument> repository;
        private readonly GraphServiceClient graphServiceClient;

        public RetrieveUserService(IDocumentRepository<UserDocument> repository,
            GraphServiceClient graphServiceClient)
        {
            this.repository = repository;
            this.graphServiceClient = graphServiceClient;
        }

        public async Task<UserClaimsInfo> GetUserClaimsInfo(ClaimsPrincipal claims)
        {
            var user = new UserClaimsInfo(claims);
            var userDoc = await repository.FindByIdAsync(user.Id);
            if (userDoc == null)
            {
                await repository.InsertOneAsync(new UserDocument
                {
                    Id = user.Id,
                    UserName = user.Name,
                    Channels = new List<string>(),
                    Connections = new List<string>()
                });
            }

            return user;
        }

        public async Task<UserInfo> GetUserAsync(string userId)
        {
            var user = await graphServiceClient.Users[userId].Request().Select(x => new
            {
                x.DisplayName,
                x.Mail
            }).GetAsync();

            return new UserInfo
            {
                Id = userId,
                Email = user.Mail,
                UserName = user.DisplayName
            };
        }
    }
}
