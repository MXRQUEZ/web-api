using System.Threading.Tasks;
using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebAPI.Controllers
{
    public sealed class AuthController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(ILogger logger, IAuthenticationService authenticationService) : base(logger)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Sign up
        /// </summary>
        /// <param name="userCredentialsDto" example=
        /// "
        /// {
        ///     Email: example_mail@example.com,
        ///     Password: Your_Password1234,
        /// }
        /// ">Sign up parameters</param>
        /// <response code="200">You are signed up with us</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="500">Can't sign up right now, come back later</response>
        [HttpPost("sign-up")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] UserCredentialsDTO userCredentialsDto)
        {
            var result = await _authenticationService.SignUpAsync(userCredentialsDto);
            return result ? Ok() : BadRequest();
        }

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="userCredentialsDto" example=
        /// "
        /// {
        ///     Email: example_mail@example.com,
        ///     Password: Your_Password1234,
        /// }
        /// ">Sign in params</param>
        /// <response code="200">Your profile was successfully changed</response>
        /// <response code="401">Wrong email or password.</response>
        /// <response code="500">Can't change your profile right now, come back later</response>
        [HttpPost("sign-in")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] UserCredentialsDTO userCredentialsDto)
        {
            var jwt = await _authenticationService.SignInAsync(userCredentialsDto);

            return jwt is null ? Unauthorized("Wrong email or password") : Ok(jwt);
        }

        /// <summary>
        /// Changes user`s password
        /// </summary>
        /// <param name="id" example="12">Your id</param>
        /// <param name="token" example="aabsrDfgs">Your token</param>
        /// <response code="204">Confirmation was successful</response>
        /// <response code="400">Bad parameters</response>
        /// <response code="500">Can't confirm your email right now, come back later</response>
        [HttpGet("email-confirmation")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(int id, string token)
        {
            var result = await _authenticationService.ConfirmEmailAsync(id, token);

            return result ? NoContent() : BadRequest("Email is unconfirmed.");
        }
    }
}
