using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography.X509Certificates;
using TradeChat.Services;

namespace TradeChat.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.ConfigureAppConfiguration((context, config) =>
                   {
                       var builtConfig = config.Build();
                       if (!string.IsNullOrEmpty(builtConfig["KeyVault:Name"]))
                       {
                           config.AddAzureKeyVault(
                               new Uri($"https://{builtConfig["KeyVaultName"]}.vault.azure.net/"),
                               new DefaultAzureCredential());
                       }
                   });

                   webBuilder.UseStartup<Startup>();
               });

        //public static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.ConfigureAppConfiguration((context, config) =>
        //            {
        //                var builtConfig = config.Build();
        //                var vaultName = builtConfig["KeyVault:KeyVaultName"];
        //                var clientId = builtConfig["KeyVault:ClientId"];
        //                var thumbprint = builtConfig["KeyVault:Thumbprint"];
        //                if (!string.IsNullOrEmpty(vaultName))
        //                {
        //                    config.AddAzureKeyVault(
        //                       $"https://{vaultName}.vault.azure.net/",
        //                        clientId, Encryption.GetCertificate(thumbprint),
        //                        new PrefixKeyVaultSecretManager("Tradechat"));

        //                }
        //            });
        //            //new DefaultAzureCredential());
        //            webBuilder.UseStartup<Startup>();
        //        });
    }
}
