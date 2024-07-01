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
    public class ExpenseSettlementServiceTests
    {
        [Fact]
        public async Task GetExpenseSettlementByIdAsync_ExistingId_ReturnsExpenseSettlement()
        {
            // Arrange
            var expenseSettlementRepositoryMock = new Mock<IExpenseSettlementRepository>();
            var expenseSettlementService = new ExpenseSettlementService(expenseSettlementRepositoryMock.Object);
            var id = 1;
            var expenseSettlement = new ExpenseSettlementEF { Id = id };

            expenseSettlementRepositoryMock.Setup(repo => repo.GetExpenseSettlementByIdAsync(id))
                                           .ReturnsAsync(expenseSettlement);

            // Act
            var result = await expenseSettlementService.GetExpenseSettlementByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetExpenseSettlementByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var expenseSettlementRepositoryMock = new Mock<IExpenseSettlementRepository>();
            var expenseSettlementService = new ExpenseSettlementService(expenseSettlementRepositoryMock.Object);
            var id = 999;

            expenseSettlementRepositoryMock.Setup(repo => repo.GetExpenseSettlementByIdAsync(id))
                                           .ReturnsAsync((ExpenseSettlementEF)null);

            // Act
            var result = await expenseSettlementService.GetExpenseSettlementByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetExpenseSettlementsAsync_ReturnsListOfExpenseSettlements()
        {
            // Arrange
            var expenseSettlementRepositoryMock = new Mock<IExpenseSettlementRepository>();
            var expenseSettlementService = new ExpenseSettlementService(expenseSettlementRepositoryMock.Object);
            var expenseSettlements = new List<ExpenseSettlementEF>
        {
            new ExpenseSettlementEF { Id = 1 },
            new ExpenseSettlementEF { Id = 2 }
        };

            expenseSettlementRepositoryMock.Setup(repo => repo.GetExpenseSettlementsAsync())
                                           .ReturnsAsync(expenseSettlements);

            // Act
            var result = await expenseSettlementService.GetExpenseSettlementsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expenseSettlements.Count, result.Count);
            Assert.Equal(expenseSettlements[0].Id, result[0].Id);
            Assert.Equal(expenseSettlements[1].Id, result[1].Id);
        }

        [Fact]
        public async Task GetExpenseSettlementsByGroupIdAsync_ExistingGroupId_ReturnsListOfExpenseSettlements()
        {
            // Arrange
            var expenseSettlementRepositoryMock = new Mock<IExpenseSettlementRepository>();
            var expenseSettlementService = new ExpenseSettlementService(expenseSettlementRepositoryMock.Object);
            var groupId = 1;
            var expenseSettlements = new List<ExpenseSettlementEF>
        {
            new ExpenseSettlementEF { Id = 1, GroupId = groupId },
            new ExpenseSettlementEF { Id = 2, GroupId = groupId }
        };

            expenseSettlementRepositoryMock.Setup(repo => repo.GetExpenseSettlementsByGroupIdAsync(groupId))
                                           .ReturnsAsync(expenseSettlements);

            // Act
            var result = await expenseSettlementService.GetExpenseSettlementsByGroupIdAsync(groupId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expenseSettlements.Count, result.Count);
            Assert.Equal(expenseSettlements[0].Id, result[0].Id);
            Assert.Equal(expenseSettlements[1].Id, result[1].Id);
        }

        // Additional tests for other methods (AddExpenseSettlementAsync, UpdateExpenseSettlementAsync, DeleteExpenseSettlementAsync) can be similarly implemented
    }
}
