using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TradeChat.Services.Plaid;
using TradeChat.Services.Plaid.Models;

namespace TradeChat.API.Worker.Controllers
{
    [Route("api/plaid")]
    [ApiController]
    public class PlaidController : ControllerBase
    {
        private readonly IGetPlaidDataService getPlaidDataService;
        private readonly IPlaidAccessTokenService accessTokenService;
        public PlaidController(
            IGetPlaidDataService getPlaidDataService,
            IPlaidAccessTokenService accessTokenService)
        {
            this.getPlaidDataService = getPlaidDataService;
            this.accessTokenService = accessTokenService;
        }

        [HttpPost("save")]
        public async Task SaveAccount([FromBody] SavePlaidLinkItem item)
        {
            await accessTokenService.SaveAsync(item);
        }

        [HttpPost("investment/{id}")]
        public async Task GetInvestmentData(int id)
        {
            await getPlaidDataService.GetInvestmentTransactionAsync(id);
        }
    }
}
