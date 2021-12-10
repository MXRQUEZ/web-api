using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Helpers;
using Business.Interfaces;
using Business.Parameters;
using DAL.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ICacheManager<User> _cacheManager;

        public UserService(IMapper mapper, UserManager<User> userManager, ICacheManager<User> cacheManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _cacheManager = cacheManager;
        }

        public async Task<IEnumerable<string>> GetUsersAsync(PageParameters pageParameters)
        {
            var users = await _userManager.Users.ToListAsync();
            var usersList = new PagedList<User>(
                users,
                pageParameters.PageNumber,
                pageParameters.PageSize);

            return usersList.Select(u => $"{u.UserName} - {u.Email}");
        }

        public async Task<UserDTO> GetUserInfoAsync(string userId)
        {
            var userCacheKey = _cacheManager.GetCacheKey(userId);
            var user = _cacheManager.GetCachedData(userCacheKey);
            if (user is not null)
                return _mapper.Map<UserDTO>(user);

            user = await _userManager.FindByIdAsync(userId);
            _cacheManager.SetCache(userCacheKey, user);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> UpdateAsync(string userId, UserDTO userDto)
        {
            var userCacheKey = _cacheManager.GetCacheKey(userId);
            _cacheManager.RemoveCache(userCacheKey);
            var oldUser = await _userManager.FindByIdAsync(userId);
            var newUser = _mapper.Map(userDto, oldUser);
            var result = await _userManager.UpdateAsync(newUser);
            return result.Succeeded ? _mapper.Map<UserDTO>(newUser) : null;
        }

        public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword,
            string confirmationPassword)
        {
            if (newPassword != confirmationPassword)
                return false;
            var user = await _userManager.FindByIdAsync(userId);

            var isRightPassword = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!isRightPassword)
                return false;

            await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return true;
        }
    }
}