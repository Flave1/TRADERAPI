using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TradeChat.Services;
using TradeChat.Services.Binance;
using TradeChat.Services.BrokerServices;
using TradeChat.Services.ChatChannel;
using TradeChat.Services.ChatMessage;
using TradeChat.Services.Coinbase;
using TradeChat.Services.Ecrypt;
using TradeChat.Services.Email.Models;
using TradeChat.Services.Extensions;
using TradeChat.Services.Gemini;
using TradeChat.Services.Gemini.Models;
using TradeChat.Services.Kraken;
using TradeChat.Services.Liquid;
using TradeChat.Services.Luno;
using TradeChat.Services.Plaid;
using TradeChat.Services.TradeServices;
using TradeChat.Services.UserServices;
using TradeChat.Services.Zabo;
using TradeChat.Services.Zabo.Models;

namespace TradeChat.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IRetrieveUserService, RetrieveUserService>();
            services.AddScoped<IChannelService, ChannelService>();
            services.AddScoped<IChannelMemberService, ChannelMemberService>();
            services.AddScoped<IChannelInvitationService, ChannelInvitationService>();
            services.AddScoped<IRetrieveMessageService, RetrieveMessageService>();
            services.AddScoped<ISendMessageService, SendMessageService>();
            services.AddScoped<IShareTradeService, ShareTradeService>();

            services.AddScoped<ILinkedBrokerService, LinkedBrokerService>();

            services.AddScoped<IZaboAuthenticationService, ZaboAuthenticationService>();
            services.AddScoped<IGetZaboBrokerService, GetZaboBrokerService>();

            services.AddScoped<IGetPlaidInstitutionService, GetPlaidInstitutionService>();
            services.AddScoped<IPlaidLinkService, PlaidLinkService>(); 

            services.AddCommonServices();
            services.AddBrokerAuthenticationServices();

            return services;
        }

        public static IServiceCollection AddBrokerAuthenticationServices(this IServiceCollection services)
        {
            services.AddScoped<ICoinbaseAuthorizationService, CoinbaseAuthenticationService>();
            services.AddScoped<IBinanceAuthenticationService, BinanceAuthenticationService>();
            services.AddSingleton<IOAuthStateManager, OAuthStateManager>();
            services.AddScoped<ILunoAuthenticationService, LunoAuthenticationService>();
            services.AddScoped<IKrakenAuthenticationService, KrakenAuthenticationService>();
            services.AddScoped<ILiquidAuthenticationService, LiquidAuthenticationService>();
            services.AddScoped<IGeminiAuthenticationService, GeminiAuthenticationService>();

            return services;
        }

        public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ZaboConfigOptions>(
                configuration.GetSection("ZaboSettings"));

            services.Configure<SendGridOptions>(
                configuration.GetSection("SendGridSettings"));
             
            services.AddBrokerConfigurations(configuration);

            return services;
        }
    }
}
