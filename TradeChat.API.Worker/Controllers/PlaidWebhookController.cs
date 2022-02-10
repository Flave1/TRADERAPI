using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;
using TradeChat.Services.Plaid;

namespace TradeChat.API.Worker.Controllers
{
    [Route("api/webhook/plaid")]
    [ApiController]
    [AllowAnonymous]
    public class PlaidWebhookController : ControllerBase
    {
        private readonly IPlaidWebhookHandlerService handlerService;

        public PlaidWebhookController(IPlaidWebhookHandlerService handlerService)
        {
            this.handlerService = handlerService;
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] JsonElement content)
        {
            var type = content.GetProperty("webhook_type").GetString();
            var data = JsonSerializer.Serialize(content);
            await handlerService.RunAsync(type, data);
            return Ok();
        }
    }
}
