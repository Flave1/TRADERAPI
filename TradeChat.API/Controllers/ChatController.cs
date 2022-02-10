using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Threading.Tasks;
using TradeChat.Data.ViewModels;
using TradeChat.Services.ChatMessage;
using TradeChat.Services.UserServices;

namespace TradeChat.API.Controllers
{
    [Route("api/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IRetrieveUserService retrieveUserService;
        private readonly ISendMessageService sendMessageService;
        public ChatController(
            ISendMessageService sendMessageService,
            IRetrieveUserService retrieveUserService
        )
        {
            this.sendMessageService = sendMessageService;
            this.retrieveUserService = retrieveUserService;
        }

        /// <summary>
        /// Post a message to a specified channel.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="message"></param>
        /// <response code="200">Message successfully posted.</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpPost("{channelId}/send")]
        [Description("Post a message to a specified channel.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendMessage(string channelId, [FromBody] CreateMessageDto message)
        {
            var user = await retrieveUserService.GetUserClaimsInfo(User);
            await sendMessageService.SendToChannel(channelId, message, user);
            return Ok();
        }

        /// <summary>
        /// Post a message to all channels the user belongs to.
        /// </summary>
        /// <param name="message"></param>
        /// <response code="200">Message successfully posted.</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpPost("broadcast")]
        [Description("Post a message to all channels the user belongs to.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BroadcastMessage([FromBody] CreateMessageDto message)
        {
            var user = await retrieveUserService.GetUserClaimsInfo(User);
            await sendMessageService.SendToAllChannels(message, user);
            return Ok();
        }
    }
}
