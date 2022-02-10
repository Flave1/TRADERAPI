using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using TradeChat.Data.ViewModels;
using TradeChat.Services.ChatChannel;
using TradeChat.Services.UserServices;

namespace TradeChat.API.Controllers
{
    [Route("api/channel")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private readonly IChannelService channelService;
        private readonly IRetrieveUserService retrieveUserService;
        public ChannelController(
            IChannelService channelService,
            IRetrieveUserService retrieveUserService
        )
        {
            this.channelService = channelService;
            this.retrieveUserService = retrieveUserService;
        }

        /// <summary>
        /// Creates a new Channel
        /// </summary>
        /// <param name="channel"></param>
        /// <returns>Saved channel information</returns>
        /// <response code="200">Returns the newly created channel.</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response> 
        [HttpPost("create")]
        [Description("Creates a new Channel.")]
        [ProducesResponseType(typeof(ChannelDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateChannel(CreateChannelDto channel)
        {
            try
            {
                var user = await retrieveUserService.GetUserClaimsInfo(User);
                var result = await channelService.CreateChannel(channel, user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Deletes an existing Channel. Only if delete action is performed by the last member
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns>Saved channel information</returns>
        /// <response code="200">Successfully deleted channel</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpDelete("{channelId}/delete")]
        [Description("Deletes an existing Channel.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteChannel(Guid channelId)
        {
            try
            {
                var user = await retrieveUserService.GetUserClaimsInfo(User);
                await channelService.DeleteChannel(channelId.ToString(), user);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Gets all channels belonging to the user.
        /// </summary>
        /// <returns>list of channels the user is subscribed to.</returns>
        /// <response code="200">Returns a list of channels the user is subscribed to.</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpGet("list")]
        [Description("Gets all channels belonging to the user.")]
        [ProducesResponseType(typeof(IEnumerable<ChannelDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListUserChannels()
        {
            try
            {
                var user = await retrieveUserService.GetUserClaimsInfo(User);
                var result = await channelService.ListChannels(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
