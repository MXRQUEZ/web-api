using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using DAL.Models;
using DAL.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Business.Services
{
    public sealed class AuthService : IAuthService
    {
        private readonly IEmailSender _emailSender;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AuthService(IMapper mapper, UserManager<User> userManager, IJwtGenerator jwtGenerator,
            IEmailSender emailSender)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtGenerator = jwtGenerator;
            _emailSender = emailSender;
        }

        public async Task<string> SignInAsync(UserCredentialsDTO userCredentialsDto)
        {
            var user = await _userManager.FindByEmailAsync(userCredentialsDto.Email);
            if (user is null)
                return await Task.FromResult<string>(null);

            var isRightPassword = await _userManager.CheckPasswordAsync(user, userCredentialsDto.Password);
            if (!isRightPassword)
                return await Task.FromResult<string>(null);

            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            if (!isEmailConfirmed)
                return await Task.FromResult<string>(null);

            return await _jwtGenerator.GenerateTokenAsync(user);
        }

        public async Task<bool> SignUpAsync(UserCredentialsDTO userCredentialsDto)
        {
            var user = _mapper.Map<User>(userCredentialsDto);

            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!result.Succeeded)
                return false;

            await _userManager.AddToRoleAsync(user, Role.User);

            await _emailSender.SendConfirmationEmailAsync(user);

            return true;
        }

        public async Task<bool> ConfirmEmailAsync(string id, string token)
        {
            var user = await _userManager.FindByIdAsync(id);
            var tokenDecodedBytes = WebEncoders.Base64UrlDecode(token);
            var tokenDecodedString = Encoding.UTF8.GetString(tokenDecodedBytes);
            var result = await _userManager.ConfirmEmailAsync(user, tokenDecodedString);

            return result.Succeeded;
        }
    }
}