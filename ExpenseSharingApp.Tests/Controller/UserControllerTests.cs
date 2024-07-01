using ExpenseSharingApp.API.Controllers;
using ExpenseSharingApp.BLL.IServices;
using ExpenseSharingApp.Common.UserDTOs;
using ExpenseSharingApp.DAL.EF;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.Tests.Controller
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _userController = new UserController(_mockUserService.Object);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsOkResult_WithAllUsers()
        {
            // Arrange
            var users = new List<UserEF>
        {
            new UserEF { Id = 1, UserName = "User1", Email = "user1@example.com", Role = "user", Balance = 0, Token = "token1" },
            new UserEF { Id = 2, UserName = "User2", Email = "user2@example.com", Role = "user", Balance = 0, Token = "token2" }
        };

            _mockUserService
                .Setup(service => service.GetAllUsers())
                .ReturnsAsync(users);

            // Act
            var result = await _userController.GetAllUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsType<List<UserEF>>(okResult.Value);
            Assert.Equal(users.Count, returnedUsers.Count);
        }

        [Fact]
        public async Task GetUser_ReturnsOkResult_WithUser()
        {
            // Arrange
            var userId = 1;
            var user = new UserEF { Id = userId, UserName = "User1", Email = "user1@example.com", Role = "user", Balance = 0, Token = "token1" };

            _mockUserService
                .Setup(service => service.GetUser(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _userController.GetUser(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<UserEF>(okResult.Value);
            Assert.Equal(userId, returnedUser.Id);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;

            _mockUserService
                .Setup(service => service.GetUser(userId))
                .ReturnsAsync((UserEF)null);

            // Act
            var result = await _userController.GetUser(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var errorMessage = Assert.IsType<string>(notFoundResult.Value);
            Assert.Equal("User not found.", errorMessage);
        }

        [Fact]
        public async Task GetUsersByGroupId_ReturnsOkResult_WithUsers()
        {
            // Arrange
            var groupId = 1;
            var users = new List<UserEF>
        {
            new UserEF { Id = 1, UserName = "User1", Email = "user1@example.com", Role = "user", Balance = 0, Token = "token1" },
            new UserEF { Id = 2, UserName = "User2", Email = "user2@example.com", Role = "user", Balance = 0, Token = "token2" }
        };

            _mockUserService
                .Setup(service => service.GetUsersByGroupIdAsync(groupId))
                .ReturnsAsync(users);

            // Act
            var result = await _userController.GetUsersByGroupId(groupId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<UserEF>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedUsers = Assert.IsType<List<UserEF>>(okResult.Value);
            Assert.Equal(users.Count, returnedUsers.Count);
        }

        [Fact]
        public async Task GetUsersByGroupId_ReturnsNotFound_WhenNoUsersExist()
        {
            // Arrange
            var groupId = 1;

            _mockUserService
                .Setup(service => service.GetUsersByGroupIdAsync(groupId))
                .ReturnsAsync(new List<UserEF>());

            // Act
            var result = await _userController.GetUsersByGroupId(groupId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<UserEF>>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}
