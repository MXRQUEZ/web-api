using System;
using System.Threading.Tasks;
using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebAPI.Controllers
{
    [ApiExplorerSettings(GroupName = "v1")]
    [AllowAnonymous]
    public sealed class AuthController : BaseController
    {
        private readonly IAuthService _authenticationService;

        public AuthController(ILogger logger, IAuthService authenticationService) : base(logger) =>
            _authenticationService = authenticationService;

        /// <summary>
        ///     Sign up
        /// </summary>
        /// <param name="userCredentialsDto">Sign up parameters</param>
        /// <response code="201">Please, verify your email</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] UserCredentialsDTO userCredentialsDto)
        {
            var result = await _authenticationService.SignUpAsync(userCredentialsDto);
            return result ? Created(new Uri(Request.GetDisplayUrl()), userCredentialsDto) : BadRequest();
        }

        /// <summary>
        ///     Sign in
        /// </summary>
        /// <param name="userCredentialsDto">Sign in parameters</param>
        /// <response code="200">You are signed in!</response>
        /// <response code="401">Wrong email or password.</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] UserCredentialsDTO userCredentialsDto)
        {
            var jwt = await _authenticationService.SignInAsync(userCredentialsDto);
            return jwt is null ? Unauthorized() : Ok(jwt);
        }

        /// <summary>
        ///     Email confirmation
        /// </summary>
        /// <param name="id" example="12">Your id</param>
        /// <param name="token" example="aabsrDfgs">Your token</param>
        /// <response code="204">Confirmation was successful</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="500">Server has some issues. Please, come back later</response>
        [HttpGet("email-confirmation")]
        public async Task<IActionResult> ConfirmEmail(string id, string token)
        {
            var result = await _authenticationService.ConfirmEmailAsync(id, token);
            return result ? NoContent() : BadRequest();
        }
    }
}