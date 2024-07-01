using ExpenseSharingApp.BLL.Services;
using ExpenseSharingApp.Common.ExpenseDTO;
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
    public class ExpenseServiceTests
    {
        [Fact]
        public async Task CreateExpenseAsync_ValidExpenseDto_ReturnsCreatedExpense()
        {
            // Arrange
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var expenseService = new ExpenseService(expenseRepositoryMock.Object);
            var expenseDto = new CreateExpenseDto
            {
                GroupId = 1,
                Description = "Test Expense",
                Amount = 100.00m,
                PaidBy = 1,
                SplitAmong = new List<ExpenseSplitDto>
            {
                new ExpenseSplitDto { UserId = 2, Amount = 50.00m, UserName = "User1" },
                new ExpenseSplitDto { UserId = 3, Amount = 50.00m, UserName = "User2" }
            }
            };
            var createdExpense = new ExpenseEF
            {
                Id = 1,
                GroupId = expenseDto.GroupId,
                Description = expenseDto.Description,
                Amount = expenseDto.Amount,
                PaidBy = expenseDto.PaidBy,
                Date = DateTime.UtcNow,
                IsSetteled = false,
                SplitAmong = expenseDto.SplitAmong.Select(split => new ExpenseSplitEF
                {
                    UserId = split.UserId,
                    Amount = split.Amount,
                    UserName = split.UserName
                }).ToList()
            };

            expenseRepositoryMock.Setup(repo => repo.CreateExpenseAsync(It.IsAny<CreateExpenseDto>()))
                                 .ReturnsAsync(createdExpense);

            // Act
            var result = await expenseService.CreateExpenseAsync(expenseDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdExpense.Id, result.Id);
            Assert.Equal(createdExpense.GroupId, result.GroupId);
            Assert.Equal(createdExpense.Description, result.Description);
            Assert.Equal(createdExpense.Amount, result.Amount);
            Assert.Equal(createdExpense.PaidBy, result.PaidBy);
            Assert.Equal(createdExpense.IsSetteled, result.IsSetteled);
            Assert.Collection(result.SplitAmong,
                item =>
                {
                    Assert.Equal(createdExpense.SplitAmong.First().UserId, item.UserId);
                    Assert.Equal(createdExpense.SplitAmong.First().Amount, item.Amount);
                    Assert.Equal(createdExpense.SplitAmong.First().UserName, item.UserName);
                },
                item =>
                {
                    Assert.Equal(createdExpense.SplitAmong.Last().UserId, item.UserId);
                    Assert.Equal(createdExpense.SplitAmong.Last().Amount, item.Amount);
                    Assert.Equal(createdExpense.SplitAmong.Last().UserName, item.UserName);
                });
        }

        [Fact]
        public async Task CreateExpenseAsync_NullExpenseDto_ReturnsNull()
        {
            // Arrange
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var expenseService = new ExpenseService(expenseRepositoryMock.Object);

            // Act
            var result = await expenseService.CreateExpenseAsync(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetExpenseAsync_ExistingId_ReturnsExpense()
        {
            // Arrange
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var expenseService = new ExpenseService(expenseRepositoryMock.Object);
            var expenseId = 1;
            var expense = new ExpenseEF { Id = expenseId };

            expenseRepositoryMock.Setup(repo => repo.GetExpenseAsync(expenseId))
                                 .ReturnsAsync(expense);

            // Act
            var result = await expenseService.GetExpenseAsync(expenseId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expenseId, result.Id);
        }

        [Fact]
        public async Task GetExpenseAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var expenseRepositoryMock = new Mock<IExpenseRepository>();
            var expenseService = new ExpenseService(expenseRepositoryMock.Object);
            var expenseId = 999;

            expenseRepositoryMock.Setup(repo => repo.GetExpenseAsync(expenseId))
                                 .ReturnsAsync((ExpenseEF)null);

            // Act
            var result = await expenseService.GetExpenseAsync(expenseId);

            // Assert
            Assert.Null(result);
        }

        // Additional tests for other methods (GetExpensesByGroupIdAsync, SettleExpenseAsync, GetExpenseSettlementsByExpenseIdAsync) can be similarly implemented
    }
}
