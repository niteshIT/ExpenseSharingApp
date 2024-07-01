using ExpenseSharingApp.Common.UserDTOs;
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
    public class AuthenticationRepositoryTests
    {
        [Fact]
        public async Task Authenticate_ValidCredentials_ReturnsUser()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ExpenseSharingContext>()
                .UseInMemoryDatabase(databaseName: "Test_Authenticate_ValidCredentials")
                .Options;

            var context = new ExpenseSharingContext(options);
            var userRepository = new AuthenticationRepository(context);

            var userDto = new UserDto
            {
                UserName = "TestUser",
                Email = "testuser@example.com",
                Password = "password123"
            };

            var userEF = new UserEF
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Password = userDto.Password
            };

            context.Users.Add(userEF);
            await context.SaveChangesAsync();

            // Act
            var result = await userRepository.Authenticate(userDto.Email, userDto.Password);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userEF.Email, result.Email);
        }

        [Fact]
        public async Task Authenticate_InvalidEmail_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ExpenseSharingContext>()
                .UseInMemoryDatabase(databaseName: "Test_Authenticate_InvalidEmail")
                .Options;

            var context = new ExpenseSharingContext(options);
            var userRepository = new AuthenticationRepository(context);

            var userDto = new UserDto
            {
                UserName = "TestUser",
                Email = "testuser@example.com",
                Password = "password123"
            };

            // Act
            var result = await userRepository.Authenticate("invalidemail@example.com", userDto.Password);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Authenticate_InvalidPassword_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ExpenseSharingContext>()
                .UseInMemoryDatabase(databaseName: "Test_Authenticate_InvalidPassword")
                .Options;

            var context = new ExpenseSharingContext(options);
            var userRepository = new AuthenticationRepository(context);

            var userDto = new UserDto
            {
                UserName = "TestUser",
                Email = "testuser@example.com",
                Password = "password123"
            };

            var userEF = new UserEF
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Password = userDto.Password
            };

            context.Users.Add(userEF);
            await context.SaveChangesAsync();

            // Act
            var result = await userRepository.Authenticate(userDto.Email, "invalidpassword");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task RegisterUser_ValidUser_SuccessfullyRegisters()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ExpenseSharingContext>()
                .UseInMemoryDatabase(databaseName: "Test_RegisterUser_ValidUser")
                .Options;

            var context = new ExpenseSharingContext(options);
            var userRepository = new AuthenticationRepository(context);

            var userDto = new UserDto
            {
                UserName = "NewUser",
                Email = "newuser@example.com",
                Password = "newpassword123"
            };

            // Act
            await userRepository.RegisterUser(userDto);

            // Assert
            var registeredUser = await context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            Assert.NotNull(registeredUser);
            Assert.Equal(userDto.UserName, registeredUser.UserName);
        }
    }
}
