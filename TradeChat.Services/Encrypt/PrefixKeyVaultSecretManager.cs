using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace TradeChat.Services
{
    public class PrefixKeyVaultSecretManager : IKeyVaultSecretManager
    {
        private readonly string prefix;

        public PrefixKeyVaultSecretManager(string prefix)
        {
            this.prefix = prefix;
        }

        public string GetKey(SecretBundle secret)
        {
            return secret.SecretIdentifier.Name.Substring(prefix.Length).Replace("--", ":");
        }

        public bool Load(SecretItem secret)
        {
            return secret.Identifier.Name.StartsWith(prefix);
        }
    }
}
