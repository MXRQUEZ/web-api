using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Exceptions;
using Business.Interfaces;
using DAL.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;

        public AuthenticationService(IMapper mapper, UserManager<User> userManager, IJwtGenerator jwtGenerator)
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

        public async Task<bool> SignUpAsync(UserCredentialsDTO userCredentialsDto)
        {
            var user = _mapper.Map<User>(userCredentialsDto);

            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!result.Succeeded)
                throw new HttpStatusException(HttpStatusCode.InternalServerError, ExceptionMessage.Fail);

            await _userManager.AddToRoleAsync(user, "user");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await EmailService.SendEmailAsync(user.Email, "Confirm your account",
                $"Please, confirm your account registration. Your userId is {user.Id}{Environment.NewLine}" +
                $"Your token:{Environment.NewLine}{token}");
            return true;
        }

        public async Task<bool> ConfirmEmailAsync(int id, string token)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id.Equals(id));
            if (user is null)
                throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.UserNotFound);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                throw new HttpStatusException(HttpStatusCode.InternalServerError, ExceptionMessage.ConfirmationFailed);
            return true;
        }
    }
}