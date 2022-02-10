using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeChat.Services.Coinbase;
using TradeChat.Services.Extensions;
using TradeChat.Services.Plaid;

namespace TradeChat.API.Worker.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IPlaidWebhookHandlerService, PlaidWebhookHandlerService>();
            services.AddScoped<IGetPlaidDataService, GetPlaidDataService>();
            services.AddScoped<IPlaidAccessTokenService, PlaidAccessTokenService>();

            services.AddScoped<ICoinbaseNotificationService, CoinbaseNotificationService>();
            services.AddScoped<ICoinbaseAccessTokenService, CoinbaseAccessTokenService>();
            services.AddScoped<IGetCoinbaseDataService, GetCoinbaseDataService>();

            services.AddCommonServices();

            return services;
        }
    }
}
