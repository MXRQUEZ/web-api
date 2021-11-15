using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Exceptions;
using Business.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace Business.Services
{
    public sealed class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public string GetUsers()
        {
            var i = 1;
            IEnumerable<User> users = _userManager.Users;
            var usersInfo = users.Select(user => $"{i++}. {user.UserName}").ToList();
            var usersInfoStr = new StringBuilder();
            foreach (var userInfo in usersInfo)
            {
                usersInfoStr.Append($"{userInfo}{Environment.NewLine}");
            }

            return usersInfoStr.ToString();
        }

        public async Task<UserDTO> UpdateUserAsync(string userId, UserDTO userDto)
        {
            var oldUser = await _userManager.FindByIdAsync(userId);
            if (oldUser is null) throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.NotFound);
            var newUser = _mapper.Map(userDto, oldUser);
            var result = await _userManager.UpdateAsync(newUser);
            return result.Succeeded 
                ? _mapper.Map<UserDTO>(newUser)
                : throw new HttpStatusException(HttpStatusCode.InternalServerError, ExceptionMessage.Failed);
        }

        public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword, string confirmationPassword)
        {
            if (newPassword != confirmationPassword) 
                throw new HttpStatusException(HttpStatusCode.BadRequest, ExceptionMessage.WrongCofirmationPassword);
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.NotFound);

            var isRightPassword = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!isRightPassword) throw new HttpStatusException(HttpStatusCode.BadRequest, ExceptionMessage.WrongPassword);

            await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return true;
        }
    }
}
