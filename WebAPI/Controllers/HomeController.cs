using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Parameters;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebAPI.Filters;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiExplorerSettings(GroupName = "v4")]
    [Authorize(Roles = Role.Admin)]
    public sealed class HomeController : BaseController
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService, ILogger logger) : base(logger) =>
            _userService = userService;

        /// <summary>
        ///     Represents users in database
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">You don't have enough rights for this request</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpGet("get-info")]
        [ServiceFilter(typeof(PagesFilerAttribute))]
        public async Task<ActionResult<IEnumerable<string>>> GetInfo([FromQuery] PageParameters pageParameters)
        {
            Logger.ForContext<HomeController>().Information("request: GetInfo");
            return Ok(await _userService.GetUsersAsync(pageParameters));
        }
    }
}