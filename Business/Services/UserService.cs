using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Exceptions;
using Business.Helpers;
using Business.Interfaces;
using Business.Parameters;
using DAL.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly CacheHelper<User> _cache;

        public UserService(IMapper mapper, UserManager<User> userManager, IMemoryCache cache)
        {
            _mapper = mapper;
            _userManager = userManager;
            _cache = new CacheHelper<User>(cache);
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

        public async Task<UserDTO> GetUserInfo(string userId)
        {
            var userCacheKey = _cache.GetCacheKey(userId);
            var user = _cache.GetCachedData(userCacheKey);
            if (user is not null)
                return _mapper.Map<UserDTO>(user);

            user = await _userManager.FindByIdAsync(userId);
            _cache.SetCache(userCacheKey, user);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<UserDTO> UpdateAsync(string userId, UserDTO userDto)
        {
            var userCacheKey = _cache.GetCacheKey(userId);
            _cache.RemoveCache(userCacheKey);
            var oldUser = await _userManager.FindByIdAsync(userId);
            var newUser = _mapper.Map(userDto, oldUser);
            await _userManager.UpdateAsync(newUser);
            return _mapper.Map<UserDTO>(newUser);
        }

        public async Task ChangePasswordAsync(string userId, string oldPassword, string newPassword,
            string confirmationPassword)
        {
            if (oldPassword.IsNullOrEmpty() || newPassword.IsNullOrEmpty() || confirmationPassword.IsNullOrEmpty())
                throw new HttpStatusException(HttpStatusCode.BadRequest, ExceptionMessage.NullValue);

            if (newPassword != confirmationPassword)
                throw new HttpStatusException(HttpStatusCode.BadRequest, ExceptionMessage.WrongCofirmationPassword);
            var user = await _userManager.FindByIdAsync(userId);

            var isRightPassword = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!isRightPassword)
                throw new HttpStatusException(HttpStatusCode.BadRequest, ExceptionMessage.WrongPassword);

            await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }
    }
}