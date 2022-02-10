using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;

namespace TradeChat.Auth.Extensions
{
    public static class JwtBearerExtensions
    {
        public static void Bind(this JwtBearerOptions options, IConfiguration configuration)
        {
            options.Audience = configuration["AzureAdB2C:ClientId"];
            options.Authority = $"https://login.microsoftonline.com/{configuration["AzureAdB2C:TenantId"]}";
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidAudience = configuration["AzureAdB2C:ClientId"],
                ValidIssuer = $"https://login.microsoftonline.com/{configuration["AzureAdB2C:TenantId"]}/v2.0"
            };
        }
    }
}
