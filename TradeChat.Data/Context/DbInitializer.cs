using System.Linq;
using TradeChat.Data.Entities;

namespace TradeChat.Data.Context
{
    public static class DbInitializer
    {
        public static void Initialize(TradeChatContext context)
        {
            context.Database.EnsureCreated();
            // context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT Plan ON");
            if (context.PlaidAccounts.Any())
            {
                return;
            }

            context.PlaidAccounts.Add(new PlaidEntity
            {
                ItemId = "8Mqq5rqQ7Pcxq9MGDv3JULZ6yzZDLMCwoxGDq",
                UserId = "7ea1f078-9d12-4a19-b8da-47c1f465c6b0",
                AccessToken = "xxx"
            });

            context.SaveChanges();
        }
    }
}
