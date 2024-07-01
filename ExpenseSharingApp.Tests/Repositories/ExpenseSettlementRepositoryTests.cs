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
    public class ExpenseSettlementRepositoryTests
    {
        private DbContextOptions<ExpenseSharingContext> GetOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<ExpenseSharingContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
        }

        [Fact]
        public async Task GetExpenseSettlementByIdAsync_ExistingId_ReturnsExpenseSettlement()
        {
            // Arrange
            var options = GetOptions("GetExpenseSettlementByIdAsync");
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new ExpenseSettlementRepository(context);

                // Seed database with test data
                var expenseSettlement = new ExpenseSettlementEF { Id = 1, GroupId = 1 }; // Adjust properties as needed
                await context.ExpenseSettlements.AddAsync(expenseSettlement);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new ExpenseSettlementRepository(context);
                var result = await repository.GetExpenseSettlementByIdAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal(1, result.GroupId); // Adjust assertions based on your model
            }
        }

        [Fact]
        public async Task GetExpenseSettlementByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var options = GetOptions("GetExpenseSettlementByIdAsync_NonExistingId");
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new ExpenseSettlementRepository(context);

                // Seed database with test data (if needed)

            }

            // Act
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new ExpenseSettlementRepository(context);
                var result = await repository.GetExpenseSettlementByIdAsync(999); // Non-existing ID

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetExpenseSettlementsAsync_ReturnsListOfExpenseSettlements()
        {
            // Arrange
            var options = GetOptions("GetExpenseSettlementsAsync");
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new ExpenseSettlementRepository(context);

                // Seed database with test data
                var expenseSettlements = new List<ExpenseSettlementEF>
                {
                    new ExpenseSettlementEF { Id = 1, GroupId = 1 },
                    new ExpenseSettlementEF { Id = 2, GroupId = 1 }
                };
                await context.ExpenseSettlements.AddRangeAsync(expenseSettlements);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new ExpenseSettlementRepository(context);
                var result = await repository.GetExpenseSettlementsAsync();

                // Assert
                Assert.Equal(2, result.Count); // Ensure correct number of items returned
            }
        }

        [Fact]
        public async Task GetExpenseSettlementsByGroupIdAsync_ExistingGroupId_ReturnsListOfExpenseSettlements()
        {
            // Arrange
            var options = GetOptions("GetExpenseSettlementsByGroupIdAsync");
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new ExpenseSettlementRepository(context);

                // Seed database with test data
                var groupId = 1;
                var expenseSettlements = new List<ExpenseSettlementEF>
                {
                    new ExpenseSettlementEF { Id = 1, GroupId = groupId },
                    new ExpenseSettlementEF { Id = 2, GroupId = groupId }
                };
                await context.ExpenseSettlements.AddRangeAsync(expenseSettlements);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new ExpenseSettlementRepository(context);
                var result = await repository.GetExpenseSettlementsByGroupIdAsync(1);

                // Assert
                Assert.Equal(2, result.Count); // Ensure correct number of items returned
                Assert.All(result, r => Assert.Equal(1, r.GroupId)); // Verify all items have the correct GroupId
            }
        }

        [Fact]
        public async Task AddExpenseSettlementAsync_ValidExpenseSettlement_AddsToDatabase()
        {
            // Arrange
            var options = GetOptions("AddExpenseSettlementAsync");
            var expenseSettlement = new ExpenseSettlementEF { Id = 1, GroupId = 1 }; // Adjust properties as needed

            // Act
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new ExpenseSettlementRepository(context);
                await repository.AddExpenseSettlementAsync(expenseSettlement);
            }

            // Assert
            using (var context = new ExpenseSharingContext(options))
            {
                var result = await context.ExpenseSettlements.FirstOrDefaultAsync();
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal(1, result.GroupId); // Adjust assertions based on your model
            }
        }

        [Fact]
        public async Task UpdateExpenseSettlementAsync_ValidExpenseSettlement_UpdatesDatabase()
        {
            // Arrange
            var options = GetOptions("UpdateExpenseSettlementAsync");
            var expenseSettlement = new ExpenseSettlementEF { Id = 1, GroupId = 1 }; // Adjust properties as needed

            using (var context = new ExpenseSharingContext(options))
            {
                await context.ExpenseSettlements.AddAsync(expenseSettlement);
                await context.SaveChangesAsync();
            }

            // Modify the expenseSettlement
            expenseSettlement.GroupId = 2; // Modify a property

            // Act
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new ExpenseSettlementRepository(context);
                await repository.UpdateExpenseSettlementAsync(expenseSettlement);
            }

            // Assert
            using (var context = new ExpenseSharingContext(options))
            {
                var result = await context.ExpenseSettlements.FindAsync(1);
                Assert.NotNull(result);
                Assert.Equal(2, result.GroupId); // Ensure the modification was saved
            }
        }

        [Fact]
        public async Task DeleteExpenseSettlementAsync_ValidId_RemovesFromDatabase()
        {
            // Arrange
            var options = GetOptions("DeleteExpenseSettlementAsync");
            var expenseSettlement = new ExpenseSettlementEF { Id = 1, GroupId = 1 }; // Adjust properties as needed

            using (var context = new ExpenseSharingContext(options))
            {
                await context.ExpenseSettlements.AddAsync(expenseSettlement);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new ExpenseSettlementRepository(context);
                await repository.DeleteExpenseSettlementAsync(1);
            }

            // Assert
            using (var context = new ExpenseSharingContext(options))
            {
                var result = await context.ExpenseSettlements.FindAsync(1);
                Assert.Null(result); // Ensure the expense settlement was deleted
            }
        }
    }
}
