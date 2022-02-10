using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using TradeChat.Data.ViewModels;
using TradeChat.Services.ChatMessage;

namespace TradeChat.API.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IRetrieveMessageService retrieveMessageService;

        public MessageController(IRetrieveMessageService retrieveMessageService)
        {
            this.retrieveMessageService = retrieveMessageService;
        }

        /// <summary>
        /// Return messages in the channel.
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns>list of messages.</returns>
        /// <response code="200">Returns a list of messages in the channel.</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpGet("{channelId}/list")]
        [Description("Return messages in the channel.")]
        [ProducesResponseType(typeof(ICollection<MessageItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListMessages(string channelId)
        {
            var result = await retrieveMessageService.ListChannelMessages(channelId);
            return Ok(result);
        }
    }
}
