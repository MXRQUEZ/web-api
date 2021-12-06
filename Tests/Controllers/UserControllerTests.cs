using System;
using System.Threading.Tasks;
using Business.DTO;
using Business.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tests.Extensions;
using Tests.Extensions.TestData;
using WebAPI.Controllers;
using Xunit;

namespace Tests.Controllers
{
    public sealed class UserControllerTests
    {
        [Fact]
        public async Task UpdateProfile_WithValidUserDto_ReturnUserDto()
        {
            //Arrange
            var validUserDto = UserControllerTestData.ValidUserDto;
            var fakeUserService = A.Fake<IUserService>();
            var userController = new UserController(fakeUserService, null)
                .WithTestUser();

            A.CallTo(() => fakeUserService.UpdateAsync(A<string>.Ignored, A<UserDTO>.Ignored))
                .Returns(Task.FromResult(new UserDTO()));

            // Act
            var actionResult = await userController.UpdateProfile(validUserDto);

            // Assert
            Assert.IsType<UserDTO>(actionResult.Value);
        }

        [Fact]
        public async Task UpdateProfile_WithInvalidUserDto_ReturnBadRequest()
        {
            //Arrange
            var fakeUserService = A.Fake<IUserService>();
            var invalidUserDto = UserControllerTestData.InvalidUserDto;
            var userController = new UserController(fakeUserService, null)
                .WithTestUser();

            A.CallTo(() => fakeUserService.UpdateAsync(A<string>.Ignored, A<UserDTO>.Ignored))
                .Returns(Task.FromResult<UserDTO>(null));

            // Act
            var actionResult = await userController.UpdateProfile(invalidUserDto);

            // Assert
            Assert.IsType<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public void UpdateProfile_CheckAuthorizedAttribute()
        {
            //Arrange
            var fakeUserService = A.Fake<IUserService>();
            var userController = new UserController(fakeUserService, null);

            // Act
            var actualAttribute = userController
                .GetType()
                .GetMethod(UserControllerTestData.UpdateProfileMethodName)!
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            //Assert
            Assert.Equal(typeof(AuthorizeAttribute), actualAttribute[0].GetType());
        }

        [Fact]
        public async Task ChangePassword_WithValidUserData_ReturnOk()
        {
            //Arrange
            var fakeUserService = A.Fake<IUserService>();
            var userController = new UserController(fakeUserService, null)
                .WithTestUser();

            A.CallTo(() => fakeUserService.ChangePasswordAsync(
                    A<string>.Ignored,
                    A<string>.Ignored,
                    A<string>.Ignored,
                    A<string>.Ignored))
                .Returns(Task.FromResult(true));

            // Act
            var actionResult = await userController.ChangePassword("_", "_", "_");

            // Assert
            Assert.IsType<OkResult>(actionResult);
        }

        [Fact]
        public async Task ChangePassword_WithInvalidUserData_ReturnBadRequest()
        {
            //Arrange
            var fakeUserService = A.Fake<IUserService>();
            var userController = new UserController(fakeUserService, null)
                .WithTestUser();

            A.CallTo(() => fakeUserService.ChangePasswordAsync(
                    A<string>.Ignored,
                    A<string>.Ignored,
                    A<string>.Ignored,
                    A<string>.Ignored))
                .Returns(Task.FromResult(false));

            // Act
            var actionResult = await userController.ChangePassword("_", "_", "_");

            // Assert
            Assert.IsType<BadRequestResult>(actionResult);
        }

        [Fact]
        public void ChangePassword_CheckAuthorizedAttribute()
        {
            //Arrange
            var fakeUserService = A.Fake<IUserService>();
            var userController = new UserController(fakeUserService, null);

            // Act
            var actualAttribute = userController
                .GetType()
                .GetMethod(UserControllerTestData.ChangePasswordMethodName)!
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            //Assert
            Assert.Equal(typeof(AuthorizeAttribute), actualAttribute[0].GetType());
        }

        [Fact]
        public async Task GetUserInfo_WithAuthorizedUser_ReturnUserDTO()
        {
            //Arrange
            var fakeUserService = A.Fake<IUserService>();
            var userController = new UserController(fakeUserService, null)
                .WithTestUser();

            A.CallTo(() => fakeUserService
                .GetUserInfoAsync(A<string>.Ignored))
                .Returns(Task.FromResult(new UserDTO()));

            // Act
            var actionResult = await userController.GetUserInfo();

            // Assert
            Assert.IsType<UserDTO>(actionResult.Value);
        }

        [Fact]
        public void GetUserInfo_CheckAuthorizedAttribute()
        {
            //Arrange
            var fakeUserService = A.Fake<IUserService>();
            var userController = new UserController(fakeUserService, null);

            // Act
            var actualAttribute = userController
                .GetType()
                .GetMethod(UserControllerTestData.GetUserInfoMethodName)!
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            //Assert
            Assert.Equal(typeof(AuthorizeAttribute), actualAttribute[0].GetType());
        }
    }
}
