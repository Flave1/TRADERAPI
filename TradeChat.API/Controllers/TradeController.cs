using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Threading.Tasks;
using TradeChat.Data.ViewModels;
using TradeChat.Services.TradeServices;

namespace TradeChat.API.Controllers
{
    [Route("api/trade")]
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly IShareTradeService shareTradeService;
        public TradeController(IShareTradeService shareTradeService)
        {
            this.shareTradeService = shareTradeService;
        }

        /// <summary>
        /// Broadcast a trade made on user account.
        /// </summary>
        /// <param name="trade"></param>
        /// <response code="200">Message successfully posted.</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpPost]
        [Description("Broadcast a trade made on user account")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostTrade([FromBody] TradeDto trade)
        {
            await shareTradeService.PostAsync(trade);
            return Ok();
        }
    }
}
