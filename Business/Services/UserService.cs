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

        public string GetUsers(PageParameters pageParameters)
        {
            var usersList = PagedList<User>.ToPagedList(
                _userManager.Users,
                pageParameters.PageNumber,
                pageParameters.PageSize);
            var i = 1;
            var usersInfo = usersList.Select(user => $"{i++}. {user.UserName} - {user.Email}");
            var usersInfoStr = new StringBuilder();
            foreach (var userInfo in usersInfo) usersInfoStr.Append($"{userInfo}\n");

            return usersInfoStr.ToString();
        }

        public async Task<UserDTO> UpdateAsync(string userId, UserDTO userDto)
        {
            var oldUser = await _userManager.FindByIdAsync(userId);
            if (oldUser is null) throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.UserNotFound);
            var newUser = _mapper.Map(userDto, oldUser);
            var result = await _userManager.UpdateAsync(newUser);
            return result.Succeeded
                ? _mapper.Map<UserDTO>(newUser)
                : throw new HttpStatusException(HttpStatusCode.InternalServerError, ExceptionMessage.Fail);
        }

        public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword,
            string confirmationPassword)
        {
            if (newPassword != confirmationPassword)
                throw new HttpStatusException(HttpStatusCode.BadRequest, ExceptionMessage.WrongCofirmationPassword);
            var user = await _userManager.FindByIdAsync(userId);

            var isRightPassword = await _userManager.CheckPasswordAsync(user, oldPassword);
            if (!isRightPassword)
                throw new HttpStatusException(HttpStatusCode.BadRequest, ExceptionMessage.WrongPassword);

            await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return true;
        }
    }
}