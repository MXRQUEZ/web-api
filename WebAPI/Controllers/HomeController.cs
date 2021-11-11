using Business.Interfaces;
using DAL.Model;
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

        [HttpGet("get-info")]
        [Authorize(Roles = Roles.ADMIN)]
        public string GetInfo()
        {
            Logger.ForContext<HomeController>().Information("request: GetInfo");
            return _userService.GetUsers();
        }
    }
}
