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

namespace Tests.Services
{
    public sealed class AuthServiceTests
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
    }
}
