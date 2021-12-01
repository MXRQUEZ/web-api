﻿using System.Linq;
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
using Microsoft.IdentityModel.Tokens;

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
            var usersList = new PagedList<User>(
                _userManager.Users,
                pageParameters.PageNumber,
                pageParameters.PageSize);

            var usersInfo = usersList.Select(user => $"{user.UserName} - {user.Email}");
            var usersInfoStr = new StringBuilder();
            foreach (var userInfo in usersInfo) 
                usersInfoStr.Append($"{userInfo}\n");

            return usersInfoStr.ToString();
        }

        public async Task<UserDTO> UpdateAsync(string userId, UserDTO userDto)
        {
            var oldUser = await _userManager.FindByIdAsync(userId);
            if (oldUser is null) throw new HttpStatusException(HttpStatusCode.NotFound, ExceptionMessage.UserNotFound);
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