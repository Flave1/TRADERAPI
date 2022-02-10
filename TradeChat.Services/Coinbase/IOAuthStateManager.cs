using System.Threading.Tasks;
using TradeChat.Services.Coinbase.Models;

namespace TradeChat.Services.Coinbase
{
    public interface IOAuthStateManager
    {
        /// <summary>
        /// Generate Random state for OAuth flow
        /// </summary>
        /// <returns></returns>
        Task<StateModel> GenerateState();

        /// <summary>
        /// Validate state against stored states
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Task ValidateState(string state);
    }
}
