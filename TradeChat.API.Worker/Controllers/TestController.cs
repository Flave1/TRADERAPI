using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TradeChat.API.Worker.Controllers
{
    [Route("api/test")]
    [ApiController]
    [AllowAnonymous]
    public class TestController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok("Hello world");
        }
    }
}
