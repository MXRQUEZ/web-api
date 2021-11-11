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

        [HttpPost("sign-up")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] UserCredentialsDTO userCredentialsDto)
        {
            var result = await _authenticationService.SignUpAsync(userCredentialsDto);
            return result ? Ok() : BadRequest();
        }


        [HttpPost("sign-in")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] UserCredentialsDTO userCredentialsDto)
        {
            var jwt = await _authenticationService.SignInAsync(userCredentialsDto);

            return jwt is null ? Unauthorized("Wrong email or password") : Ok(jwt);
        }

        [HttpGet("email-confirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(int id, string token)
        {
            var result = await _authenticationService.ConfirmEmailAsync(id, token);

            return result ? NoContent() : BadRequest("Email is unconfirmed.");
        }
    }
}
