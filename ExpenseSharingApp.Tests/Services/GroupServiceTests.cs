using ExpenseSharingApp.BLL.Services;
using ExpenseSharingApp.Common.GroupDTOs;
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
    public class GroupServiceTests
    {
        [Fact]
        public async Task CreateGroupAsync_ValidGroup_ReturnsCreatedGroup()
        {
            // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var groupService = new GroupService(groupRepositoryMock.Object);

            var groupDto = new CreateGroupDto
            {
                Name = "Test Group",
                Description = "Test Description",
                Members = new List<UserGroupDto>
            {
                new UserGroupDto { UserId = 1, Role = "Admin" },
                new UserGroupDto { UserId = 2, Role = "Member" }
            }
            };

            var createdGroupEF = new GroupEF
            {
                Id = 1,
                Name = groupDto.Name,
                Description = groupDto.Description,
                CreatedDate = DateTime.UtcNow,
                Members = groupDto.Members.Select(m => new UserGroupEF { UserId = m.UserId, Role = m.Role }).ToList()
            };

            groupRepositoryMock.Setup(repo => repo.CreateGroupAsync(It.IsAny<GroupEF>()))
                              .ReturnsAsync(createdGroupEF);

            // Act
            var result = await groupService.CreateGroupAsync(createdGroupEF);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createdGroupEF.Id, result.Id);
            Assert.Equal(groupDto.Name, result.Name);
            Assert.Equal(groupDto.Description, result.Description);
            Assert.Equal(groupDto.Members.Count, result.Members.Count);
            
        }

       
        [Fact]
        public async Task GetGroupByIdAsync_ExistingId_ReturnsGroup()
        {
            // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var groupService = new GroupService(groupRepositoryMock.Object);

            var groupId = 1;
            var groupEF = new GroupEF
            {
                Id = groupId,
                Name = "Test Group",
                Description = "Test Description",
                CreatedDate = DateTime.UtcNow,
                Members = new List<UserGroupEF>
            {
                new UserGroupEF { UserId = 1, Role = "Admin" },
                new UserGroupEF { UserId = 2, Role = "Member" }
            }
            };

            groupRepositoryMock.Setup(repo => repo.GetGroupByIdAsync(groupId))
                              .ReturnsAsync(groupEF);

            // Act
            var result = await groupService.GetGroupByIdAsync(groupId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(groupId, result.Id);
            Assert.Equal(groupEF.Name, result.Name);
            Assert.Equal(groupEF.Description, result.Description);
            Assert.Equal(groupEF.Members.Count, result.Members.Count);
            
        }

        [Fact]
        public async Task GetGroupByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var groupService = new GroupService(groupRepositoryMock.Object);

            var groupId = 999; // Non-existing ID

            groupRepositoryMock.Setup(repo => repo.GetGroupByIdAsync(groupId))
                              .ReturnsAsync((GroupEF)null);

            // Act
            var result = await groupService.GetGroupByIdAsync(groupId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetGroupsByUserIdAsync_ExistingUserId_ReturnsListOfGroups()
        {
            // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var groupService = new GroupService(groupRepositoryMock.Object);

            var userId = 1;
            var groupsEF = new List<GroupEF>
        {
            new GroupEF
            {
                Id = 1,
                Name = "Test Group 1",
                Description = "Test Description 1",
                CreatedDate = DateTime.UtcNow,
                Members = new List<UserGroupEF>
                {
                    new UserGroupEF { UserId = userId, Role = "Admin" }
                }
            },
            new GroupEF
            {
                Id = 2,
                Name = "Test Group 2",
                Description = "Test Description 2",
                CreatedDate = DateTime.UtcNow,
                Members = new List<UserGroupEF>
                {
                    new UserGroupEF { UserId = userId, Role = "Member" }
                }
            }
        };

            groupRepositoryMock.Setup(repo => repo.GetGroupsByUserId(userId))
                              .ReturnsAsync(groupsEF);

            // Act
            var result = await groupService.GetGroupsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(groupsEF.Count, result.Count);
            Assert.Equal(groupsEF[0].Id, result[0].Id);
            Assert.Equal(groupsEF[1].Id, result[1].Id);
        }

        [Fact]
        public async Task GetGroupsByUserIdAsync_NonExistingUserId_ReturnsEmptyList()
        {
            // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var groupService = new GroupService(groupRepositoryMock.Object);

            var userId = 999; // Non-existing user ID

            groupRepositoryMock.Setup(repo => repo.GetGroupsByUserId(userId))
                              .ReturnsAsync(new List<GroupEF>());

            // Act
            var result = await groupService.GetGroupsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllGroupsAsync_ReturnsAllGroups()
        {
            // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var groupService = new GroupService(groupRepositoryMock.Object);

            var groupsEF = new List<GroupEF>
        {
            new GroupEF
            {
                Id = 1,
                Name = "Test Group 1",
                Description = "Test Description 1",
                CreatedDate = DateTime.UtcNow,
                Members = new List<UserGroupEF>
                {
                    new UserGroupEF { UserId = 1, Role = "Admin" }
                }
            },
            new GroupEF
            {
                Id = 2,
                Name = "Test Group 2",
                Description = "Test Description 2",
                CreatedDate = DateTime.UtcNow,
                Members = new List<UserGroupEF>
                {
                    new UserGroupEF { UserId = 2, Role = "Member" }
                }
            }
        };

            groupRepositoryMock.Setup(repo => repo.GetAllGroupsAsync())
                              .ReturnsAsync(groupsEF);

            // Act
            var result = await groupService.GetAllGroupsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(groupsEF.Count, result.Count());
            Assert.Equal(groupsEF[0].Id, result.ElementAt(0).Id);
            Assert.Equal(groupsEF[1].Id, result.ElementAt(1).Id);
        }

        [Fact]
        public async Task GetAllGroupsAsync_EmptyGroups_ReturnsEmptyList()
        {
            // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var groupService = new GroupService(groupRepositoryMock.Object);

            groupRepositoryMock.Setup(repo => repo.GetAllGroupsAsync())
                              .ReturnsAsync(new List<GroupEF>());

            // Act
            var result = await groupService.GetAllGroupsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task DeleteGroupAsync_ExistingGroupId_DeletesGroup()
        {
            // Arrange
            var groupRepositoryMock = new Mock<IGroupRepository>();
            var groupService = new GroupService(groupRepositoryMock.Object);

            var groupId = 1;

            groupRepositoryMock.Setup(repo => repo.DeleteGroupAsync(groupId))
                              .Returns(Task.CompletedTask);

            // Act
            await groupService.DeleteGroupAsync(groupId);

            // Assert
            groupRepositoryMock.Verify(repo => repo.DeleteGroupAsync(groupId), Times.Once);
        }

        

        
    }
}
