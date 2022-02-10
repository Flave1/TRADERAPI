using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using TradeChat.Data.ViewModels;
using TradeChat.Data.ViewModels.Plaid;
using TradeChat.Data.ViewModels.Zabo;
using TradeChat.Services.BrokerServices;
using TradeChat.Services.Plaid;
using TradeChat.Services.UserServices;

namespace TradeChat.API.Controllers
{
    [Route("api/brokers")]
    [ApiController]
    public class BrokerController : ControllerBase
    {
        private readonly ILinkedBrokerService linkedBrokerService;
        private readonly IRetrieveUserService retrieveUserService;
        private readonly IPlaidLinkService plaidLinkService;

        public BrokerController(
            ILinkedBrokerService linkedBrokerService,
            IRetrieveUserService retrieveUserService,
            IPlaidLinkService plaidLinkService
        )
        {
            this.linkedBrokerService = linkedBrokerService;
            this.retrieveUserService = retrieveUserService;
            this.plaidLinkService = plaidLinkService;
        }

        /// <summary>
        /// Gets a list of integrated forex brokers/providers. (Not implemented)
        /// </summary>
        /// <returns>list of channels the user is subscribed to.</returns>
        /// <response code="200">Not implemented yet.</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpGet("forex")]
        [Description("Gets a list of integrated forex brokers/providers.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetForexBrokers()
        {
            throw new NotImplementedException("Coming Soon.");
        }

        /// <summary>
        /// Callback endpoint for Zabo client widget after successful integration.
        /// </summary>
        /// <response code="200">Account record successfully created.</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpPost("zabo")]
        [Description("Callback endpoint for Zabo client widget after successful integration.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task SaveLinkedZaboAccount([FromBody] ZaboUserAccount account)
        {
            var user = await retrieveUserService.GetUserClaimsInfo(User);
            await linkedBrokerService.LinkAsync(account, user);
        }

        /// <summary>
        /// Callback endpoint for Plaid client widget after successful integration.
        /// </summary>
        /// <response code="200">Account record successfully created.</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpPost("plaid")]
        [Description("Callback endpoint for Plaid client widget after successful integration.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task SaveLinkedPlaidAccount([FromBody] SavePlaidUserAccount account)
        {
            var user = await retrieveUserService.GetUserClaimsInfo(User);
            await linkedBrokerService.LinkAsync(account, user);
        }

        /// <summary>
        /// Gets a plaid link token for the plaid widget.
        /// </summary>
        /// <returns>a token</returns>
        /// <response code="200">Returns a token to the client</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpGet("plaid/token")]
        [ProducesResponseType(typeof(GetPlaidTokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPlaidLinkToken()
        {
            var user = await retrieveUserService.GetUserClaimsInfo(User);
            var token = await plaidLinkService.GetLinkTokenAsync(user);
            return Ok(new GetPlaidTokenResponse
            {
                Token = token
            });
        }

        /// <summary>
        /// Gets a list of linked brokers for the user.
        /// </summary>
        /// <returns>list of channels the user is subscribed to.</returns>
        /// <response code="200">Returns a list of brokers for the user</response>
        /// <response code="400">If a required parameter is missing.</response> 
        /// <response code="500">Server Error</response>
        [HttpGet("linked")]
        [Description("Gets a list of linked brokers for the user.")]
        [ProducesResponseType(typeof(ICollection<BrokerDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLinkedBrokers()
        {
            var user = await retrieveUserService.GetUserClaimsInfo(User);
            var result = await linkedBrokerService.GetAsync(user);
            return Ok(result);
        }
    }
}
