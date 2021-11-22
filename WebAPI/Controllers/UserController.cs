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
        /// <param name="userDto" example="
        /// {
        ///     Email: example_mail@example.com,
        ///     UserName: YourNickName,
        ///     PhoneNumber: 375292678085,
        ///     DateOfBirth: 28/03/2002,
        ///     AddressDelivery: Minsk, Voronyanskogo,
        ///     Gender: Male
        /// }
        /// ">Profile update params</param>
        /// <response code="200">Your profile was successfully changed</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Can't change your profile right now, come back later</response>
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
        /// <response code="200">Password was successfully changed</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Can't change your password right now, come back later</response>
        [HttpPatch("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword, string confirmationPassword)
        {
            await _userService.ChangePasswordAsync(UserHelper.GetIdByClaims(User.Claims), oldPassword, newPassword, confirmationPassword);
            return Ok();
        }
    }
}
