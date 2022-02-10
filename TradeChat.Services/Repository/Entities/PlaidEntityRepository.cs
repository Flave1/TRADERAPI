using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TradeChat.Data.Context;
using TradeChat.Data.Entities;
using TradeChat.Services.Repository.Entities;

namespace TradeChat.Services.Repository
{
    public class PlaidEntityRepository : EntityRepository<PlaidEntity>, IPlaidEntityRepository
    {
        public PlaidEntityRepository(TradeChatContext context) : base(context)
        {

        }

        public override async Task<PlaidEntity> AddAsync(PlaidEntity entity)
        {
            var existingItem = await context.PlaidAccounts.FirstOrDefaultAsync(x => x.UserId == entity.UserId && x.ItemId == entity.ItemId);
            if (existingItem != null)
            {
                existingItem.AccessToken = entity.AccessToken;
                existingItem.Updated = DateTime.UtcNow;
                existingItem.Archived = false;
                return await base.UpdateAsync(existingItem);
            }

            return await base.AddAsync(entity);
        }

        public async Task<PlaidEntity> GetAccountByItemIdAsync(string id)
        {
            var item = await context.PlaidAccounts.FirstOrDefaultAsync(x => x.ItemId == id);
            return item;
        }
    }
}
