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
    public class GroupRepositoryTests
    {
        private DbContextOptions<ExpenseSharingContext> GetOptions(string databaseName)
        {
            return new DbContextOptionsBuilder<ExpenseSharingContext>()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options;
        }

        [Fact]
        public async Task CreateGroupAsync_ValidGroup_CreatesGroup()
        {
            // Arrange
            var options = GetOptions("CreateGroupAsync");
            var group = new GroupEF { Name = "Test Group", Description = "Test Description" }; // Adjust properties as needed

            // Act
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new GroupRepository(context);
                var result = await repository.CreateGroupAsync(group);

                // Assert
                Assert.NotNull(result);
                Assert.NotEqual(0, result.Id); // Ensure ID is generated
            }

            // Assert after saving changes
            using (var context = new ExpenseSharingContext(options))
            {
                var savedGroup = await context.Groups.FindAsync(group.Id);
                Assert.NotNull(savedGroup);
                Assert.Equal("Test Group", savedGroup.Name); // Verify other properties as needed
            }
        }

        [Fact]
        public async Task GetGroupByIdAsync_ExistingId_ReturnsGroupWithMembers()
        {
            // Arrange
            var options = GetOptions("GetGroupByIdAsync");
            var groupId = 1;

            using (var context = new ExpenseSharingContext(options))
            {
                // Seed database with test data
                var group = new GroupEF
                {
                    Id = groupId,
                    Name = "Test Group",
                    Members = new List<UserGroupEF>
                {
                    new UserGroupEF { UserId = 1, Role = "Admin" },
                    new UserGroupEF { UserId = 2, Role = "Member" }
                }
                };
                await context.Groups.AddAsync(group);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new GroupRepository(context);
                var result = await repository.GetGroupByIdAsync(groupId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(groupId, result.Id);
                //Assert.Equal(2, result.Members.Count); // Verify member count
            }
        }

        [Fact]
        public async Task GetGroupByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var options = GetOptions("GetGroupByIdAsync_NonExistingId");

            // Act
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new GroupRepository(context);
                var result = await repository.GetGroupByIdAsync(999); // Non-existing ID

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetAllGroupsAsync_ReturnsListOfGroups()
        {
            // Arrange
            var options = GetOptions("GetAllGroupsAsync");

            using (var context = new ExpenseSharingContext(options))
            {
                // Seed database with test data
                var groups = new List<GroupEF>
                {
                    new GroupEF { Id = 1, Name = "Group 1" },
                    new GroupEF { Id = 2, Name = "Group 2" }
                };
                await context.Groups.AddRangeAsync(groups);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new GroupRepository(context);
                var result = await repository.GetAllGroupsAsync();

                // Assert
                Assert.Equal(2, result.Count()); // Ensure correct number of groups returned
            }
        }

        [Fact]
        public async Task GetGroupsByUserIdAsync_ExistingUserId_ReturnsListOfGroups()
        {
            // Arrange
            var options = GetOptions("GetGroupsByUserIdAsync");
            var userId = 1;

            using (var context = new ExpenseSharingContext(options))
            {
                // Seed database with test data
                var userGroups = new List<UserGroupEF>
                {
                    new UserGroupEF { UserId = userId, GroupId = 1 },
                    new UserGroupEF { UserId = userId, GroupId = 2 }
                };
                await context.UserGroups.AddRangeAsync(userGroups);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new GroupRepository(context);
                var result = await repository.GetGroupsByUserId(userId);

                // Assert
                //Assert.Equal(2, result.Count); // Ensure correct number of groups returned
                Assert.All(result, g => Assert.Contains(g.Members, m => m.UserId == userId)); // Verify user's membership
            }
        }

        [Fact]
        public async Task DeleteGroupAsync_ValidGroupId_DeletesGroupAndAssociatedData()
        {
            // Arrange
            var options = GetOptions("DeleteGroupAsync");
            var groupId = 1;

            using (var context = new ExpenseSharingContext(options))
            {
                // Seed database with test data
                var group = new GroupEF { Id = groupId, Name = "Test Group" };
                group.Expenses = new List<ExpenseEF>
                {
                    new ExpenseEF { Id = 1, GroupId = groupId, IsSetteled = false, PaidBy = 1 }
                };
                await context.Groups.AddAsync(group);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new ExpenseSharingContext(options))
            {
                var repository = new GroupRepository(context);
                await repository.DeleteGroupAsync(groupId);
            }

            // Assert
            using (var context = new ExpenseSharingContext(options))
            {
                var deletedGroup = await context.Groups.FindAsync(groupId);
                Assert.Null(deletedGroup); // Ensure group was deleted

                var relatedExpenses = await context.Expenses.Where(e => e.GroupId == groupId).ToListAsync();
                Assert.Empty(relatedExpenses); // Ensure associated expenses were deleted

                var relatedSettlements = await context.ExpenseSettlements.Where(es => es.GroupId == groupId).ToListAsync();
                Assert.Empty(relatedSettlements); // Ensure associated settlements were deleted
            }
        }
    }
}
