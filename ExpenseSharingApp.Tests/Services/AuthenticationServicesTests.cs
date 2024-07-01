using ExpenseSharingApp.BLL.Services;
using ExpenseSharingApp.Common.UserDTOs;
using ExpenseSharingApp.DAL.EF;
using ExpenseSharingApp.DAL.IRepository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.Tests.Services
{
    public class AuthenticationServicesTests
    {
        private readonly Mock<IAuthenticationRepository> _mockAuthRepository;
        private readonly AuthenticationServices _authenticationService;

        public AuthenticationServicesTests()
        {
            _mockAuthRepository = new Mock<IAuthenticationRepository>();
            _authenticationService = new AuthenticationServices(_mockAuthRepository.Object);
        }

        [Fact]
        public async Task Authenticate_ValidCredentials_ReturnsUserResponseDto()
        {
            // Arrange
            string email = "test@example.com";
            string password = "password";
            var user = new UserEF
            {
                Id = 1,
                UserName = "TestUser",
                Email = email,
                Role = "user",
                Balance = 100
                // Add more properties as needed
            };
            _mockAuthRepository.Setup(repo => repo.Authenticate(email, password)).ReturnsAsync(user);

            // Act
            var result = await _authenticationService.Authenticate(email, password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.Role, result.Role);
            Assert.Equal(user.Balance, result.Balance);

            // Additional assertions for token creation if needed
            Assert.NotNull(result.Token);
        }

        [Fact]
        public async Task Authenticate_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            string email = "test@example.com";
            string password = "password";
            _mockAuthRepository.Setup(repo => repo.Authenticate(email, password)).ReturnsAsync((UserEF)null);

            // Act
            var result = await _authenticationService.Authenticate(email, password);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RegisterUser_ValidUserDto_CallsRepository()
        {
            // Arrange
            var userDto = new UserDto
            {
                UserName = "NewUser",
                Email = "newuser@example.com",
                Password = "newpassword",
                Role = "user"
                // Add more properties as needed
            };

            // Act
            await _authenticationService.RegisterUser(userDto);

            // Assert
            _mockAuthRepository.Verify(repo => repo.RegisterUser(It.IsAny<UserDto>()), Times.Once);
        }
    }
}
