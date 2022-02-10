using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using TradeChat.Data.ViewModels;
using TradeChat.Models.ViewModels;
using TradeChat.Services.ChatChannel;
using TradeChat.Services.UserServices;

namespace TradeChat.API.Controllers
{
    [Route("api/member")]
    [ApiController]
    public class ChannelMemberController : ControllerBase
    {
        private readonly IRetrieveUserService retrieveUserService;
        private readonly IChannelMemberService channelMemberService;
        private readonly IChannelInvitationService invitationService;

        public ChannelMemberController(
            IRetrieveUserService retrieveUserService,
            IChannelMemberService channelMemberService,
            IChannelInvitationService invitationService
        )
        {
            this.invitationService = invitationService;
            this.retrieveUserService = retrieveUserService;
            this.channelMemberService = channelMemberService;
        }

        /// <summary>
        /// Get all members of a channel.
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns>list of channels the user is subscribed to.</returns>
        /// <response code="200">Returns a list of users in the specified channel.</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpGet("{channelId}/list")]
        [Description("Get all members of a channel.")]
        [ProducesResponseType(typeof(ICollection<ChannelMemberDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ListMembers(string channelId)
        {
            var result = await channelMemberService.ListMembers(channelId);
            return Ok(result);
        }

        /// <summary>
        /// Send out an email invitation to join a specified channel.
        /// </summary>
        /// <param name="request"></param> 
        /// <response code="200">Inivitation email successfully sent.</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpPost("{channelId}/invite")]
        [Description("Send out an email invitation to join a specified channel.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InviteUser([FromBody] InviteMemberRequest request)
        {
            try
            { 
                var user = await retrieveUserService.GetUserClaimsInfo(User);
                await invitationService.InviteAsync(request, user);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Add invited user to channel through using invitation code.
        /// </summary>
        /// <param name="code"></param>
        /// <response code="200">User successfully added to channel.</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpPost("add/{code}")]
        [Description("Add invited user to channel through using invitation code.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMember(string code)
        {
            try
            {
                //validate invitation code
                var user = await retrieveUserService.GetUserClaimsInfo(User);
                await invitationService.RedeemInvitationAsync(code, user);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// Remove user from the specified channel.
        /// </summary>
        /// <param name="channelId"></param>
        /// <response code="200">User successfully left channel.</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpPost("{channelId}/leave")]
        [Description("Remove user from the specified channel.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LeaveChannel(string channelId)
        {
            try
            {
                var user = await retrieveUserService.GetUserClaimsInfo(User);
                await channelMemberService.LeaveChannel(channelId, user);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
