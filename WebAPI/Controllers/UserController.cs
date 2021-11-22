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

        /// <summary>
        /// Updates user profile
        /// </summary>
        /// <param name="userDto">Profile update params</param>
        /// <response code="200">Profile was updated</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPut("update-profile")]
        [Authorize]
        public async Task<UserDTO> UpdateProfile([FromBody] UserDTO userDto)
        {
            return await _userService.UpdateAsync(UserHelper.GetIdByClaims(User.Claims), userDto);
        }

        /// <summary>
        /// Changes user`s password
        /// </summary>
        /// <param name="oldPassword" example="_SkJwNif2345">Old password</param>
        /// <param name="newPassword" example="_SkJwNif23456">New password</param>
        /// <param name="confirmationPassword" example="_SkJwNif23456">Your password confirmation</param>
        /// <response code="200">Password was changed</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPatch("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword, string confirmationPassword)
        {
            await _userService.ChangePasswordAsync(
                UserHelper.GetIdByClaims(User.Claims), oldPassword, newPassword, confirmationPassword);
            return Ok();
        }
    }
}
