using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;
using TradeChat.Auth.Services;
using TradeChat.Data;
using TradeChat.Data.Context;
using TradeChat.Models.Options;
using TradeChat.Services.Binance.Models;
using TradeChat.Services.Coinbase.Models;
using TradeChat.Services.Ecrypt;
using TradeChat.Services.Email;
using TradeChat.Services.Email.Sendgrid;
using TradeChat.Services.Kraken.Models;
using TradeChat.Services.Liquid.Models;
using TradeChat.Services.Luno.Models;
using TradeChat.Services.Plaid.Models;
using TradeChat.Services.Repository;
using TradeChat.Services.Repository.Entities;
using TradeChat.Services.WebRequestHelper;

namespace TradeChat.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBrokerConfigurations(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<PlaidOptions>(
                configuration.GetSection("PlaidSettings"));

            services.Configure<CoinbaseOptions>(
               configuration.GetSection("CoinbaseSettings"));

            services.Configure<BinanceOptions>(
               configuration.GetSection("BinanceSettings"));

            services.Configure<LunoOptions>(
               configuration.GetSection("LunoSettings"));

            services.Configure<KrakenOptions>(
               configuration.GetSection("KrakenSettings"));

            services.Configure<LiquidOptions>(
             configuration.GetSection("LiquidSettings"));

            return services;
        }

        public static IServiceCollection AddDocumentDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ChatDatabaseSettings>(
                configuration.GetSection(nameof(ChatDatabaseSettings)));

            services.Configure<TradeChat.Models.Options.ApplicationOptions>(
                configuration.GetSection("ApplicationSettings"));

            services.Configure<EncryptionOptions>(
                configuration.GetSection("EncryptionSettings"));

            services.Configure<KeyVaultConfig>(
              configuration.GetSection("KeyVault"));


            services.AddSingleton<IChatDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<ChatDatabaseSettings>>().Value);

            services.AddSingleton<IDatabaseInitializationService, DatabaseInitializationService>();
            services.AddScoped(typeof(IDocumentRepository<>), typeof(DocumentRepository<>));

            return services;
        }

        public static IServiceCollection AddEntityDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TradeChatContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));

            services.AddTransient(typeof(IEntityRepository<>), typeof(EntityRepository<>));
            services.AddTransient<IPlaidEntityRepository, PlaidEntityRepository>();
            services.AddTransient<ICoinbaseEntityRepository, CoinbaseEntityRepository>();

            return services;
        }

        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            services.AddHttpClient();

            services.AddScoped<IWebRequestService, WebRequestService>();

            services.AddScoped<ISendEmailService, SendGridService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IAccessTokenService, AccessTokenService>();

            return services;
        }

        public static IServiceCollection AddGraphServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<GraphServiceClient>(sp =>
            {
                //// Initialize the client credential auth provider
                var scopes = new string[] { "https://graph.microsoft.com/.default" };
                var confidentialClient = ConfidentialClientApplicationBuilder
                    .Create(configuration["AzureAdB2C:ClientId"])
                    .WithAuthority($"https://login.microsoftonline.com/{configuration["AzureAdB2C:TenantId"]}/v2.0")
                    .WithClientSecret(configuration["AzureAdB2C:Secret"])
                    .Build();



                // Set up the Microsoft Graph service client with client credentials
                GraphServiceClient graphServiceClient =
                    new GraphServiceClient(new DelegateAuthenticationProvider(async (requestMessage) =>
                    {

                        // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
                        var authResult = await confidentialClient.AcquireTokenForClient(scopes).ExecuteAsync();

                        // Add the access token in the Authorization header of the API
                        requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                    }));

                return graphServiceClient;
            });

            return services;
        }
    }
}
