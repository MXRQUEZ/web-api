using System.Threading.Tasks;
using Business.Services;
using DAL.Models.Entities;
using FakeItEasy;
using Microsoft.AspNetCore.Identity;
using Xunit;
using static Tests.Extensions.TestData.FakeTestData;
using static Tests.Extensions.TestData.UserTestData;

namespace Tests.Services
{
    public sealed class AuthServiceTests
    {
        [Fact]
        public async Task SignInAsync_WithValidUserCredentials_ReturnToken()
        {
            // Arrange
            var authService = new AuthService(FakeMapper, FakeUserManager, FakeJwtGenerator, FakeEmailSender);

            A.CallTo(() => FakeUserManager.FindByEmailAsync(A<string>.Ignored))
                .Returns(Task.FromResult(new User()));

            A.CallTo(() => FakeUserManager.CheckPasswordAsync(A<User>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult(true));

            A.CallTo(() => FakeUserManager.IsEmailConfirmedAsync(A<User>.Ignored))
                .Returns(Task.FromResult(true));

            A.CallTo(() => FakeJwtGenerator.GenerateTokenAsync(A<User>.Ignored))
                .Returns(Task.FromResult(JwtTokenEncoded));

            // Act
            var result = await authService.SignInAsync(UserCredentialsDto);

            // Assert
            Assert.Equal(JwtTokenEncoded, result);
        }

        [Fact]
        public async Task SignInAsync_WithInvalidEmail_ReturnNull()
        {
            // Arrange
            var authService = new AuthService(FakeMapper, FakeUserManager, FakeJwtGenerator, FakeEmailSender);

            A.CallTo(() => FakeUserManager.FindByEmailAsync(A<string>.Ignored))
                .Returns(Task.FromResult<User>(null));

            // Act
            var result = await authService.SignInAsync(UserCredentialsDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SignInAsync_WithWrongPassword_ReturnNull()
        {
            // Arrange
            var authService = new AuthService(FakeMapper, FakeUserManager, FakeJwtGenerator, FakeEmailSender);

            A.CallTo(() => FakeUserManager.FindByEmailAsync(A<string>.Ignored))
                .Returns(Task.FromResult(new User()));

            A.CallTo(() => FakeUserManager.CheckPasswordAsync(A<User>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult(false));

            A.CallTo(() => FakeUserManager.IsEmailConfirmedAsync(A<User>.Ignored))
                .Returns(Task.FromResult(true));

            // Act
            var result = await authService.SignInAsync(UserCredentialsDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SignInAsync_WithUnconfirmedEmail_ReturnsNull()
        {
            // Arrange
            var authService = new AuthService(FakeMapper, FakeUserManager, FakeJwtGenerator, FakeEmailSender);

            A.CallTo(() => FakeUserManager.FindByEmailAsync(A<string>.Ignored))
                .Returns(Task.FromResult(new User()));

            A.CallTo(() => FakeUserManager.CheckPasswordAsync(A<User>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult(true));

            A.CallTo(() => FakeUserManager.IsEmailConfirmedAsync(A<User>.Ignored))
                .Returns(Task.FromResult(false));

            // Act
            var result = await authService.SignInAsync(UserCredentialsDto);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task SignUpAsync_WithValidUserCredentialsDto_ReturnTrue()
        {
            // Arrange
            var authService = new AuthService(FakeMapper, FakeUserManager, FakeJwtGenerator, FakeEmailSender);

            A.CallTo(() => FakeUserManager.CreateAsync(A<User>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult(IdentityResult.Success));

            A.CallTo(() => FakeEmailSender.SendConfirmationEmailAsync(A<User>.Ignored))
                .Returns(Task.CompletedTask);

            // Act
            var result = await authService.SignUpAsync(UserCredentialsDto);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task SignUpAsync_WithRegisteredUser_ReturnFalse()
        {
            // Arrange
            var authService = new AuthService(FakeMapper, FakeUserManager, FakeJwtGenerator, FakeEmailSender);

            A.CallTo(() => FakeUserManager.CreateAsync(A<User>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult(IdentityResult.Failed()));

            // Act
            var result = await authService.SignUpAsync(UserCredentialsDto);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task ConfirmEmailAsync_WithSignedUpUser_ReturnTrue()
        {
            // Arrange
            var authService = new AuthService(FakeMapper, FakeUserManager, FakeJwtGenerator, FakeEmailSender);

            A.CallTo(() => FakeUserManager.FindByIdAsync(A<string>.Ignored))
                .Returns(new User());

            A.CallTo(() => FakeUserManager.ConfirmEmailAsync(A<User>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult(IdentityResult.Success));

            // Act
            var result = await authService.ConfirmEmailAsync(UserId, JwtTokenEncoded);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task ConfirmEmailAsync_WithUnsignedUser_ReturnFalse()
        {
            // Arrange
            var authService = new AuthService(FakeMapper, FakeUserManager, FakeJwtGenerator, FakeEmailSender);

            A.CallTo(() => FakeUserManager.ConfirmEmailAsync(A<User>.Ignored, A<string>.Ignored))
                .Returns(Task.FromResult(IdentityResult.Failed()));

            // Act
            var result = await authService.ConfirmEmailAsync(UserId, JwtTokenEncoded);

            // Assert
            Assert.False(result);
        }
    }
}