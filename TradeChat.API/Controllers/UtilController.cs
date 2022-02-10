using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradeChat.Services.Plaid;
using TradeChat.Services.Zabo;

namespace TradeChat.API.Controllers
{
    [Route("api/util")]
    [ApiController, AllowAnonymous]
    public class UtilController : ControllerBase
    {
        private readonly IGetZaboBrokerService getZaboBroker;
        private readonly IGetPlaidInstitutionService getPlaidInstitution;
        public UtilController(
            IGetZaboBrokerService getZaboBroker,
            IGetPlaidInstitutionService getPlaidInstitution
        )
        {
            this.getZaboBroker = getZaboBroker;
            this.getPlaidInstitution = getPlaidInstitution;
        }

        [HttpPost("testwebhook")]
        public IActionResult TestWebhookUrl(object param = null)
        {
            return Ok(param);
        }
    }
}
