using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TradeChat.Services.Coinbase;
using TradeChat.Services.Coinbase.Models.Webhook;

namespace TradeChat.API.Worker.Controllers
{
    [Route("api/webhook/coinbase")]
    [ApiController]
    [AllowAnonymous]
    public class CoinbaseWebhookController : ControllerBase
    {
        private readonly ICoinbaseNotificationService notificationService;

        public CoinbaseWebhookController(ICoinbaseNotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public IActionResult Index()
        {
            return Ok("Hello coinbase");
        }

        [HttpPost]
        public async Task NotifyUpdate(CoinbaseNotificationDto notificationDto)
        {
            await notificationService.RunAsync(notificationDto);
        }
    }
}
