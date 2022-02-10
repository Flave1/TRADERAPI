using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeChat.Models.ViewModels.Exceptions;
using TradeChat.Services.Coinbase.Models;

namespace TradeChat.Services.Coinbase
{
    /// <summary>
    /// IOAuthStateManager implementation
    /// </summary>
    public class OAuthStateManager : IOAuthStateManager
    {
        // In-Memory states store
        public List<StateModel> States = new List<StateModel>();

        public Task<StateModel> GenerateState()
        {
            var randState = new StateModel
            {
                State = Guid.NewGuid().ToString()
            };
            SaveState(randState);
            return Task.FromResult(randState);
        }
        private Task SaveState(StateModel state)
        {
            States.Add(state);
            return Task.CompletedTask;
        }

        public async Task ValidateState(string state)
        {
            if (!await Task.FromResult(States.Any(s => s.State == state)))
            {
                throw new InvalidStateException("Unable to validate state");
            }
        }
    }
}
