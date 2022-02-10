using Microsoft.EntityFrameworkCore;
using TradeChat.Data.Entities;

namespace TradeChat.Data.Context
{
    public class TradeChatContext : DbContext
    {
        public TradeChatContext(DbContextOptions<TradeChatContext> options) : base(options)
        {
        }

        public DbSet<PlaidEntity> PlaidAccounts { get; set; }
        public DbSet<CoinbaseEntity> CoinbaseAccounts { get; set; }
    }
}
