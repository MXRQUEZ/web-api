using System;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;

        private readonly IJwtGenerator _jwtGenerator;

        public AuthenticationService(IMapper mapper, UserManager<User> userManager, IJwtGenerator jwtGenerator)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<string> SignInAsync(UserCredentialsDTO userCredentialsDto)
        {
            var user = await _userManager.FindByEmailAsync(userCredentialsDto.Email);
            if (user == null) return null;

            var isRightPassword = await _userManager.CheckPasswordAsync(user, userCredentialsDto.Password);
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return isRightPassword && isEmailConfirmed ? await _jwtGenerator.GenerateTokenAsync(user) : null;
        }

        public async Task<bool> SignUpAsync(UserCredentialsDTO userCredentialsDto)
        {
            var user = _mapper.Map<User>(userCredentialsDto);

            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!result.Succeeded) return false;

            await _userManager.AddToRoleAsync(user, "user");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await EmailService.SendEmailAsync(user.Email, "Confirm your account",
                $"Please, confirm your account registration. Your userId is {user.Id}{Environment.NewLine}" +
                $"Your token:{Environment.NewLine}{token}");
            return true;
        }

        public async Task<bool> ConfirmEmailAsync(int id, string token)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
            {
                return false;
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }
    }
}
