using ExpenseSharingApp.API.Controllers;
using ExpenseSharingApp.BLL.IServices;
using ExpenseSharingApp.Common.ExpenseDTO;
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
    public class ExpenseControllerTests
    {
        private readonly Mock<IExpenseService> _mockExpenseService;
        private readonly ExpenseController _expenseController;

        public ExpenseControllerTests()
        {
            _mockExpenseService = new Mock<IExpenseService>();
            _expenseController = new ExpenseController(_mockExpenseService.Object);
        }

        [Fact]
        public async Task CreateExpense_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var expenseDto = new CreateExpenseDto
            {
                GroupId = 1,
                Description = "Test Expense",
                Amount = 100,
                PaidBy = 1,
                SplitAmong = new List<ExpenseSplitDto>
            {
                new ExpenseSplitDto { UserId = 1, Amount = 50, UserName = "User1" },
                new ExpenseSplitDto { UserId = 2, Amount = 50, UserName = "User2" }
            }
            };

            var createdExpense = new ExpenseEF
            {
                Id = 1,
                GroupId = 1,
                Description = "Test Expense",
                Amount = 100,
                PaidBy = 1,
                Date = DateTime.Now,
                IsSetteled = false,
                SplitAmong = new List<ExpenseSplitEF>
            {
                new ExpenseSplitEF { Id = 1, ExpenseId = 1, UserId = 1, Amount = 50, UserName = "User1" },
                new ExpenseSplitEF { Id = 2, ExpenseId = 1, UserId = 2, Amount = 50, UserName = "User2" }
            }
            };

            _mockExpenseService
                .Setup(service => service.CreateExpenseAsync(expenseDto))
                .ReturnsAsync(createdExpense);

            // Act
            var result = await _expenseController.CreateExpense(expenseDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetExpense", createdResult.ActionName);
            Assert.Equal(1, ((ExpenseEF)createdResult.Value).Id);
        }

        [Fact]
        public async Task GetExpense_ReturnsOkResult_WithExpense()
        {
            // Arrange
            var expenseId = 1;
            var expense = new ExpenseEF
            {
                Id = expenseId,
                GroupId = 1,
                Description = "Test Expense",
                Amount = 100,
                PaidBy = 1,
                Date = DateTime.Now,
                IsSetteled = false,
                SplitAmong = new List<ExpenseSplitEF>
            {
                new ExpenseSplitEF { Id = 1, ExpenseId = expenseId, UserId = 1, Amount = 50, UserName = "User1" },
                new ExpenseSplitEF { Id = 2, ExpenseId = expenseId, UserId = 2, Amount = 50, UserName = "User2" }
            }
            };

            _mockExpenseService
                .Setup(service => service.GetExpenseAsync(expenseId))
                .ReturnsAsync(expense);

            // Act
            var result = await _expenseController.GetExpense(expenseId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expenseId, ((ExpenseEF)okResult.Value).Id);
        }

        [Fact]
        public async Task GetExpense_ReturnsNotFound_WhenExpenseDoesNotExist()
        {
            // Arrange
            var expenseId = 1;

            _mockExpenseService
                .Setup(service => service.GetExpenseAsync(expenseId))
                .ReturnsAsync((ExpenseEF)null);

            // Act
            var result = await _expenseController.GetExpense(expenseId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetExpensesByGroupId_ReturnsOkResult_WithExpenses()
        {
            // Arrange
            var groupId = 1;
            var expenses = new List<ExpenseEF>
        {
            new ExpenseEF { Id = 1, GroupId = groupId, Description = "Expense1", Amount = 100, PaidBy = 1, Date = DateTime.Now, IsSetteled = false },
            new ExpenseEF { Id = 2, GroupId = groupId, Description = "Expense2", Amount = 200, PaidBy = 2, Date = DateTime.Now, IsSetteled = false }
        };

            _mockExpenseService
                .Setup(service => service.GetExpensesByGroupIdAsync(groupId))
                .ReturnsAsync(expenses);

            // Act
            var result = await _expenseController.GetExpensesByGroupId(groupId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ExpenseEF>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(expenses.Count, ((List<ExpenseEF>)okResult.Value).Count);
        }

        [Fact]
        public async Task GetExpensesByGroupId_ReturnsNotFound_WhenNoExpensesExist()
        {
            // Arrange
            var groupId = -1;

            _mockExpenseService
                .Setup(service => service.GetExpensesByGroupIdAsync(groupId))
                .ReturnsAsync(new List<ExpenseEF>());

            // Act
            var result = await _expenseController.GetExpensesByGroupId(groupId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ExpenseEF>>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task SettleExpense_ReturnsOkResult()
        {
            // Arrange
            var expenseId = 1;
            var request = new SettleExpenseRequest { SettledByUserId = 1 };

            _mockExpenseService
                .Setup(service => service.SettleExpenseAsync(expenseId, request))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _expenseController.SettleExpense(expenseId, request);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task SettleExpense_ReturnsBadRequest_ForInvalidIds()
        {
            // Arrange
            var expenseId = -1;
            var request = new SettleExpenseRequest { SettledByUserId = -1 };

            // Act
            var result = await _expenseController.SettleExpense(expenseId, request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid expense ID or user ID.", badRequestResult.Value);
        }
    }
}
