using System.Threading.Tasks;
using Business.DTO;
using Business.Helpers;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebAPI.Controllers
{
    public sealed class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService, ILogger logger) : base(logger)
        {
            _userService = userService;
        }

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<UserDTO> UpdateProfile([FromBody] UserDTO userDto)
        {
            return await _userService.UpdateUserAsync(UserHelper.GetIdByClaims(User.Claims), userDto);
        }

        [HttpPatch("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            var result = await _userService.ChangePasswordAsync(UserHelper.GetIdByClaims(User.Claims), oldPassword, newPassword);
            return result ? Ok() : BadRequest();
        }
    }
}
