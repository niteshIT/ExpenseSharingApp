using ExpenseSharingApp.BLL.Services;
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
    public class UserServiceTests
    {
        [Fact]
        public async Task GetUser_ValidUserId_ReturnsUser()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object);

            var userId = 1;
            var userEF = new UserEF
            {
                Id = userId,
                UserName = "TestUser",
                Email = "testuser@example.com",
                Role = "user",
                Balance = 100
            };

            userRepositoryMock.Setup(repo => repo.GetUser(userId))
                              .ReturnsAsync(userEF);

            // Act
            var result = await userService.GetUser(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
            Assert.Equal(userEF.UserName, result.UserName);
            Assert.Equal(userEF.Email, result.Email);
            Assert.Equal(userEF.Role, result.Role);
            Assert.Equal(userEF.Balance, result.Balance);
        }

        [Fact]
        public async Task GetUser_NonExistingUserId_ReturnsNull()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object);

            var userId = 999; // Non-existing user ID

            userRepositoryMock.Setup(repo => repo.GetUser(userId))
                              .ReturnsAsync((UserEF)null);

            // Act
            var result = await userService.GetUser(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object);

            var usersEF = new List<UserEF>
        {
            new UserEF
            {
                Id = 1,
                UserName = "User1",
                Email = "user1@example.com",
                Role = "user",
                Balance = 50
            },
            new UserEF
            {
                Id = 2,
                UserName = "User2",
                Email = "user2@example.com",
                Role = "admin",
                Balance = 100
            }
        };

            userRepositoryMock.Setup(repo => repo.GetAllUsers())
                              .ReturnsAsync(usersEF);

            // Act
            var result = await userService.GetAllUsers();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usersEF.Count, result.Count());
            Assert.Equal(usersEF[0].Id, result.ElementAt(0).Id);
            Assert.Equal(usersEF[1].Id, result.ElementAt(1).Id);
        }

        [Fact]
        public async Task GetUsersByGroupIdAsync_ExistingGroupId_ReturnsUsers()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object);

            var groupId = 1;
            var usersEF = new List<UserEF>
        {
            new UserEF
            {
                Id = 1,
                UserName = "User1",
                Email = "user1@example.com",
                Role = "user",
                Balance = 50
            },
            new UserEF
            {
                Id = 2,
                UserName = "User2",
                Email = "user2@example.com",
                Role = "admin",
                Balance = 100
            }
        };

            userRepositoryMock.Setup(repo => repo.GetUsersByGroupIdAsync(groupId))
                              .ReturnsAsync(usersEF);

            // Act
            var result = await userService.GetUsersByGroupIdAsync(groupId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(usersEF.Count, result.Count());
            Assert.Equal(usersEF[0].Id, result.ElementAt(0).Id);
            Assert.Equal(usersEF[1].Id, result.ElementAt(1).Id);
        }

        [Fact]
        public async Task GetUsersByGroupIdAsync_NonExistingGroupId_ReturnsEmptyList()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var userService = new UserService(userRepositoryMock.Object);

            var groupId = 999; // Non-existing group ID

            userRepositoryMock.Setup(repo => repo.GetUsersByGroupIdAsync(groupId))
                              .ReturnsAsync(new List<UserEF>());

            // Act
            var result = await userService.GetUsersByGroupIdAsync(groupId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
