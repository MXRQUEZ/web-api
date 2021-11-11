using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using DAL.Interfaces;
using DAL.Model;
using Microsoft.AspNetCore.Identity;

namespace Business.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(IRepository<User> userRepository, IMapper mapper, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public string GetUsers()
        {
            var i = 1;
            var users = _userRepository.GetAllUsers().Select(user => $"{i++}. {user.UserName}").ToList();
            var usersInfo = new StringBuilder();
            foreach (var userInfo in users)
            {
                usersInfo.Append($"{userInfo}{Environment.NewLine}");
            }

            return usersInfo.ToString();
        }

        public async Task<UserDTO> UpdateUserAsync(string userId, UserDTO userDto)
        {
            var oldUser = await _userManager.FindByIdAsync(userId);
            if (oldUser is null) return null;
            var newUser = _mapper.Map(userDto, oldUser);
            var result = await _userManager.UpdateAsync(newUser);
            return result.Succeeded ? _mapper.Map<UserDTO>(newUser) : null;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return false;

            var isRightPassword = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!isRightPassword) return false;

            await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return true;

        }
    }
}
