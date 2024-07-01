using ExpenseSharingApp.Common.ExpenseDTO;
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
    public class ExpenseRepositoryTests
    {
        [Fact]
        public async Task CreateExpenseAsync_ValidExpense_ReturnsCreatedExpense()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ExpenseSharingContext>()
                .UseInMemoryDatabase(databaseName: "Test_CreateExpenseAsync_ValidExpense")
                .Options;

            var context = new ExpenseSharingContext(options);
            var expenseRepository = new ExpenseRepository(context);

            var expenseDto = new CreateExpenseDto
            {
                GroupId = 1,
                Description = "Test Expense",
                Amount = 100,
                PaidBy = 1,
                SplitAmong = new List<ExpenseSplitDto>
            {
                new ExpenseSplitDto { UserId = 2, Amount = 50, UserName = "User2" },
                new ExpenseSplitDto { UserId = 3, Amount = 50, UserName = "User3" }
            }
            };

            // Act
            var result = await expenseRepository.CreateExpenseAsync(expenseDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expenseDto.GroupId, result.GroupId);
            Assert.Equal(expenseDto.Description, result.Description);
            Assert.Equal(expenseDto.Amount, result.Amount);
            Assert.Equal(expenseDto.PaidBy, result.PaidBy);
            Assert.Equal(expenseDto.SplitAmong.Count, result.SplitAmong.Count);
        }

        
        [Fact]
        public async Task SettleExpenseAsync_InvalidExpense_ThrowsException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ExpenseSharingContext>()
                .UseInMemoryDatabase(databaseName: "Test_SettleExpenseAsync_InvalidExpense")
                .Options;

            var context = new ExpenseSharingContext(options);
            var expenseRepository = new ExpenseRepository(context);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => expenseRepository.SettleExpenseAsync(999, 2)); // Non-existent expense Id
        }

        [Fact]
        public async Task GetExpenseAsync_ExistingExpenseId_ReturnsExpense()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ExpenseSharingContext>()
                .UseInMemoryDatabase(databaseName: "Test_GetExpenseAsync_ExistingExpenseId")
                .Options;

            var context = new ExpenseSharingContext(options);
            var expenseRepository = new ExpenseRepository(context);

            var expense = new ExpenseEF
            {
                Id = 1,
                GroupId = 1,
                Description = "Test Expense",
                Amount = 100,
                PaidBy = 1,
                SplitAmong = new List<ExpenseSplitEF>
            {
                new ExpenseSplitEF { UserId = 2, Amount = 50, UserName = "User2" },
                new ExpenseSplitEF { UserId = 3, Amount = 50, UserName = "User3" }
            }
            };

            context.Expenses.Add(expense);
            await context.SaveChangesAsync();

            // Act
            var result = await expenseRepository.GetExpenseAsync(expense.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expense.Id, result.Id);
            Assert.Equal(expense.Description, result.Description);
            Assert.Equal(expense.Amount, result.Amount);
            Assert.Equal(expense.PaidBy, result.PaidBy);
            Assert.Equal(expense.SplitAmong.Count, result.SplitAmong.Count);
        }

        [Fact]
        public async Task GetExpenseAsync_NonExistingExpenseId_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ExpenseSharingContext>()
                .UseInMemoryDatabase(databaseName: "Test_GetExpenseAsync_NonExistingExpenseId")
                .Options;

            var context = new ExpenseSharingContext(options);
            var expenseRepository = new ExpenseRepository(context);

            // Act
            var result = await expenseRepository.GetExpenseAsync(999); // Non-existent expense Id

            // Assert
            Assert.Null(result);
        }
    }
}
