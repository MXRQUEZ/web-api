using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Business.DTO;
using Business.Exceptions;
using Business.Interfaces;
using Business.JWT;
using DAL.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace Business.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly JwtGenerator _jwtGenerator;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        private const int Port = 8080;
        private const string Host = "localhost";
        private const string Scheme = "http";
        private const string AuthPath = "api/Auth/email-confirmation";

        private readonly UserManager<User> _userManager;

        public AuthService(IMapper mapper, UserManager<User> userManager, JwtGenerator jwtGenerator, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _configuration = configuration;
        }

        public async Task<string> SignInAsync(UserCredentialsDTO userCredentialsDto)
        {
            var user = await _userManager.FindByEmailAsync(userCredentialsDto.Email);
            if (user == null) throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.WrongEmail);

            var isRightPassword = await _userManager.CheckPasswordAsync(user, userCredentialsDto.Password);
            if (!isRightPassword)
                throw new HttpStatusException(HttpStatusCode.BadRequest, ExceptionMessage.WrongPassword);

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
                throw new HttpStatusException(HttpStatusCode.Unauthorized, ExceptionMessage.Unauthorized);

            return await _jwtGenerator.GenerateTokenAsync(user);
        }

        public async Task SignUpAsync(UserCredentialsDTO userCredentialsDto)
        {
            var result = await _userManager.FindByEmailAsync(userCredentialsDto.Email);
            if (result is not null)
                throw new HttpStatusException(HttpStatusCode.BadRequest,
                    $"{ExceptionMessage.EmailAlreadyRegistered}. {userCredentialsDto.Email}");

            var user = _mapper.Map<User>(userCredentialsDto);

            await _userManager.CreateAsync(user, user.PasswordHash);
            await _userManager.AddToRoleAsync(user, "user");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var tokenEncoded = WebEncoders.Base64UrlEncode(tokenBytes);

            var scheme = _configuration.GetValue<string>("AuthURI:Scheme");
            var host = _configuration.GetValue<string>("AuthURI:Host");
            var authPath = _configuration.GetValue<string>("AuthURI:Path");
            var port = _configuration.GetValue<int>("AuthURI:Port");

            var confirmationUri = new UriBuilder
            {
                Scheme = scheme,
                Host = host,
                Path = authPath,
                Port = port
            };
            var query = HttpUtility.ParseQueryString(confirmationUri.Query);
            query["id"] = user.Id.ToString();
            query["token"] = tokenEncoded;
            confirmationUri.Query = query.ToString()!;

            await EmailService.SendEmailAsync(user.Email, "Confirm your account",
                $"Verify your account by clicking the <a href='{confirmationUri}'>link</a>");
        }

        public async Task ConfirmEmailAsync(string id, string token)
        {
            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(token);
            var tokenDecodedString = Encoding.UTF8.GetString(tokenDecodedBytes);

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.UserNotFound);

            await _userManager.ConfirmEmailAsync(user, tokenDecodedString);
        }
    }
}