using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using TradeChat.Services.Zabo.Models;

namespace TradeChat.Services.Zabo
{
    public class ZaboAuthenticationService : IZaboAuthenticationService
    {
        private readonly ZaboConfigOptions config;
        public ZaboAuthenticationService(IOptions<ZaboConfigOptions> options)
        {
            config = options.Value;
        }

        public Dictionary<string, string> Authenticate(string url)
        {
            string unixTimestamp = ((Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds).ToString();
            string signedKey = Encryption.HMACSHA256Sign(new string[2] { unixTimestamp, url }, config.Secret);

            return new Dictionary<string, string>
            {
                {"X-Zabo-Key", config.ApiKey },
                {"X-Zabo-Sig", signedKey },
                {"X-Zabo-Timestamp", unixTimestamp }
            };
        }
    }
}
