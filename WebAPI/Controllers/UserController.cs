using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Business.DTO;
using Business.Helpers;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "v2")]
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
        /// <response code="400">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPut("update-profile")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> UpdateProfile([FromBody] UserDTO userDto)
        {
            var result = await _userService.UpdateAsync(User.Claims.GetUserId(), userDto);
            return result is null ? BadRequest() : result;
        }

        /// <summary>
        /// Changes user`s password
        /// </summary>
        /// <param name="oldPassword" example="_SkJwNif2345">Old password</param>
        /// <param name="newPassword" example="_SkJwNif23456">New password</param>
        /// <param name="confirmationPassword" example="_SkJwNif23456">Your password confirmation</param>
        /// <response code="200">Password was changed</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPatch("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(
            [Required] string oldPassword, [Required] string newPassword, [Required] string confirmationPassword)
        {
            var result = await _userService.ChangePasswordAsync(
                User.Claims.GetUserId(), oldPassword, newPassword, confirmationPassword);
            return result ? Ok() : BadRequest();
        }

        /// <summary>
        /// Represents user info
        /// </summary>
        /// <response code="200">Profile info was represented</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpGet("user")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetUserInfo() =>
            await _userService.GetUserInfoAsync(User.Claims.GetUserId());
    }
}
