using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TradeChat.Data.Context;
using TradeChat.Data.Entities;

namespace TradeChat.Services.Repository.Entities
{
    public class CoinbaseEntityRepository : EntityRepository<CoinbaseEntity>, ICoinbaseEntityRepository
    {
        public CoinbaseEntityRepository(TradeChatContext context) : base(context)
        {

        }

        public override async Task<CoinbaseEntity> AddAsync(CoinbaseEntity entity)
        {
            var existingEntity = await context.CoinbaseAccounts.FirstOrDefaultAsync(x => x.UserId == entity.UserId);
            if (existingEntity != null)
            {
                throw new Exception("Account already exists");
            }

            await context.CoinbaseAccounts.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<CoinbaseEntity> FindByBrokerAccount(string accountId)
        {
            var account = await context.CoinbaseAccounts.FirstOrDefaultAsync(x => x.BrokerAccountId == accountId);
            return account;
        }
    }
}
