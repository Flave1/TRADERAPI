using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TradeChat.Data.MessageQueueModels;
using TradeChat.Services.MessageQueue;
using TradeChat.Services.MessageQueue.Configuration;
using TradeChat.Services.MessageQueue.Provider;

namespace TradeChat.Services.Extensions
{
    public static class MessageServiceCollectionExtensions
    {
        public static IServiceCollection AddMessagingServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MessageQueueConfigOptions>(
                configuration.GetSection("MessageQueueSettings"));

            services.TryAddSingleton<IMessageQueueProvider, MessageQueueProvider>();
            services.AddScoped<ISendQueueMessageService<PlaidInvestmentQueueMessage>, PlaidInvestmentQueueMessageService>();
            services.AddScoped<ISendQueueMessageService<PostTradeQueueMessage>, PostTradeQueueMessageService>();

            return services;
        }
    }
}
