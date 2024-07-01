using ExpenseSharingApp.API.Controllers;
using ExpenseSharingApp.BLL.IServices;
using ExpenseSharingApp.Common.UserDTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.Tests.Controller
{
    public class AuthenticationControllerTests
    {
        private readonly Mock<IAuthenticationServices> _mockAuthService;
        private readonly AuthenticationController _controller;

        public AuthenticationControllerTests()
        {
            _mockAuthService = new Mock<IAuthenticationServices>();
            _controller = new AuthenticationController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Authenticate_ReturnsBadRequest_WhenUserDtoIsNull()
        {
            // Arrange
            UserDto userDto = null;

            // Act
            var result = await _controller.Authenticate(userDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Authenticate_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            var userDto = new UserDto { Email = "test@example.com", Password = "password" };
            _mockAuthService.Setup(service => service.Authenticate(userDto.Email, userDto.Password)).ReturnsAsync((UserResponseDto)null);

            // Act
            var result = await _controller.Authenticate(userDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorMessage = Assert.IsType<string>(notFoundResult.Value);
            Assert.Equal("User Not Found", errorMessage);
        }

        [Fact]
        public async Task Authenticate_ReturnsOk_WhenUserFound()
        {
            // Arrange
            var userDto = new UserDto { Email = "test@example.com", Password = "password" };
            var userResponseDto = new UserResponseDto
            {
                Id = 1,
                UserName = "testuser",
                Email = "test@example.com",
                Role = "user",
                Balance = 100.0m,
                Token = "some-token"
            };
            _mockAuthService.Setup(service => service.Authenticate(userDto.Email, userDto.Password)).ReturnsAsync(userResponseDto);

            // Act
            var result = await _controller.Authenticate(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedUserDto = okResult.Value as UserResponseDto;
            Assert.NotNull(returnedUserDto);
            Assert.Equal(userResponseDto.Id, returnedUserDto.Id);
            Assert.Equal(userResponseDto.UserName, returnedUserDto.UserName);
            Assert.Equal(userResponseDto.Email, returnedUserDto.Email);
            Assert.Equal(userResponseDto.Role, returnedUserDto.Role);
            Assert.Equal(userResponseDto.Balance, returnedUserDto.Balance);
            Assert.Equal(userResponseDto.Token, returnedUserDto.Token);
        }
            [Fact]
        public async Task Register_ReturnsBadRequest_WhenUserDtoIsNull()
        {
            // Arrange
            UserDto userDto = null;

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenUserIsRegistered()
        {
            // Arrange
            var userDto = new UserDto
            {
                UserName = "newuser",
                Email = "newuser@example.com",
                Password = "password",
                Role = "user"
            };
            _mockAuthService.Setup(service => service.RegisterUser(userDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Register(userDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("User registered successfully.", ((dynamic)okResult.Value));
        }
    }
}
