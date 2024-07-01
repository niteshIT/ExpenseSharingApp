using ExpenseSharingApp.DAL.Data;
using ExpenseSharingApp.DAL.EF;
using ExpenseSharingApp.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.Tests.Repositories
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task GetUser_ValidUserId_ReturnsUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ExpenseSharingContext>()
                .UseInMemoryDatabase(databaseName: "ExpenseSharing_Test_GetUser")
                .Options;

            using (var context = new ExpenseSharingContext(options))
            {
                var user = new UserEF { Id = 1, UserName = "TestUser", Email = "testuser@example.com" };
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new UserRepository(context);

                // Act
                var result = await repository.GetUser(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal("TestUser", result.UserName);
                Assert.Equal("testuser@example.com", result.Email);
            }
        }

        [Fact]
        public async Task GetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ExpenseSharingContext>()
                .UseInMemoryDatabase(databaseName: "ExpenseSharing_Test_GetAllUsers")
                .Options;

            using (var context = new ExpenseSharingContext(options))
            {
                context.Users.AddRange(
                    new UserEF { Id = 1, UserName = "User1", Email = "user1@example.com" },
                    new UserEF { Id = 2, UserName = "User2", Email = "user2@example.com" }
                );
                await context.SaveChangesAsync();
            }

            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new UserRepository(context);

                // Act
                var result = await repository.GetAllUsers();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count()); // Assuming two users were added
                Assert.Contains(result, u => u.UserName == "User1");
                Assert.Contains(result, u => u.UserName == "User2");
            }
        }

        [Fact]
        public async Task GetUsersByGroupIdAsync_ValidGroupId_ReturnsUsers()
        {
            // Arrange
            var groupId = 1;
            var options = new DbContextOptionsBuilder<ExpenseSharingContext>()
                .UseInMemoryDatabase(databaseName: "ExpenseSharing_Test_GetUsersByGroupId")
                .Options;

            using (var context = new ExpenseSharingContext(options))
            {
                var user1 = new UserEF { Id = 1, UserName = "User1", Email = "user1@example.com" };
                var user2 = new UserEF { Id = 2, UserName = "User2", Email = "user2@example.com" };
                var userGroup1 = new UserGroupEF { UserId = 1, GroupId = groupId, Role = "Admin" };
                var userGroup2 = new UserGroupEF { UserId = 2, GroupId = groupId, Role = "Member" };

                context.Users.AddRange(user1, user2);
                context.UserGroups.AddRange(userGroup1, userGroup2);
                await context.SaveChangesAsync();
            }

            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new UserRepository(context);

                // Act
                var result = await repository.GetUsersByGroupIdAsync(groupId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count()); // Assuming two users were added to the group
                Assert.Contains(result, u => u.UserName == "User1");
                Assert.Contains(result, u => u.UserName == "User2");
            }
        }
    }
}
