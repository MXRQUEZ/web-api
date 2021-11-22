using Business.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public sealed class HomeController : BaseController
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService, ILogger logger) : base(logger)
        {
            _userService = userService;
        }

        /// <summary>
        /// Changes user`s password
        /// </summary>
        /// <response code="200">User were represented successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">You don't have rights for this request</response>
        /// <response code="500">Come back later</response>
        [HttpGet("get-info")]
        [Authorize(Roles = Roles.ADMIN)]
        public string GetInfo()
        {
            Logger.ForContext<HomeController>().Information("request: GetInfo");
            return _userService.GetUsers();
        }
    }
}
