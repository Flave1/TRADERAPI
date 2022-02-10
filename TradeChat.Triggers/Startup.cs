using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using TradeChat.Auth.Services;
using TradeChat.Models;
using TradeChat.Models.Options;
using TradeChat.Triggers.Services.WebRequestHelper;
//using TradeChat.Services.Extensions;

[assembly: FunctionsStartup(typeof(TradeChat.Triggers.Startup))]
namespace TradeChat.Triggers
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.Configure<AzureADOptions>(options =>
            {
                options.ClientId = Environment.GetEnvironmentVariable("ClientId");
                options.Secret = Environment.GetEnvironmentVariable("ClientSecret");
                options.Domain = Environment.GetEnvironmentVariable("TenantDomain");
            });
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IWebRequestService, WebRequestService>();
            builder.Services.AddScoped<IAccessTokenService, AccessTokenService>();
            //throw new NotImplementedException();
            //builder.Services.AddCommonServices();
        }
    }
}
