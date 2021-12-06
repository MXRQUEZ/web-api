using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Business.DTO;
using Business.Exceptions;
using Business.Helpers;
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
            if (user is null) 
                return null;

            var isRightPassword = await _userManager.CheckPasswordAsync(user, userCredentialsDto.Password);
            if (!isRightPassword)
                return null;

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
                return null;

            return await _jwtGenerator.GenerateTokenAsync(user);
        }

        public async Task<bool> SignUpAsync(UserCredentialsDTO userCredentialsDto)
        {
            var result = await _userManager.FindByEmailAsync(userCredentialsDto.Email);
            if (result is not null)
                return false;

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

            await EmailSender.SendEmailAsync(user.Email, "Confirm your account",
                $"Verify your account by clicking the <a href='{confirmationUri}'>link</a>");

            return true;
        }

        public async Task<bool> ConfirmEmailAsync(string id, string token)
        {
            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(token);
            var tokenDecodedString = Encoding.UTF8.GetString(tokenDecodedBytes);

            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return false;

            await _userManager.ConfirmEmailAsync(user, tokenDecodedString);
            return true;
        }
    }
}