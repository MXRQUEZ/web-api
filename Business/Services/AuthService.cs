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

namespace Business.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly JwtGenerator _jwtGenerator;
        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;

        public AuthService(IMapper mapper, UserManager<User> userManager, JwtGenerator jwtGenerator)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
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
            var user = _mapper.Map<User>(userCredentialsDto);

            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!result.Succeeded)
                throw new HttpStatusException(HttpStatusCode.InternalServerError, ExceptionMessage.Fail);

            await _userManager.AddToRoleAsync(user, "user");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var tokenEncoded = WebEncoders.Base64UrlEncode(tokenBytes);

            const int port = 44340;
            const string localhost = "localhost";
            const string scheme = "https";
            const string authPath = "api/Auth/email-confirmation";

            var confirmationUri = new UriBuilder
            {
                Port = port,
                Host = localhost,
                Scheme = scheme,
                Path = authPath
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

            var result = await _userManager.ConfirmEmailAsync(user, tokenDecodedString);
            if (!result.Succeeded)
                throw new HttpStatusException(HttpStatusCode.InternalServerError, ExceptionMessage.ConfirmationFailed);
        }
    }
}