using ExpenseSharingApp.API.Controllers;
using ExpenseSharingApp.BLL.IServices;
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
    public class GroupControllerTests
    {
        private readonly Mock<IGroupService> _mockGroupService;
        private readonly Mock<IExpenseSettlementService> _mockExpenseSettlementService;
        private readonly GroupController _controller;

        public GroupControllerTests()
        {
            _mockGroupService = new Mock<IGroupService>();
            _mockExpenseSettlementService = new Mock<IExpenseSettlementService>();
            _controller = new GroupController(_mockGroupService.Object, _mockExpenseSettlementService.Object);
        }

        [Fact]
        public async Task CreateGroup_ReturnsBadRequest_WhenGroupIsInvalid()
        {
            // Arrange
            GroupEF invalidGroup = null;

            // Act
            var result = await _controller.CreateGroup(invalidGroup);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateGroup_ReturnsCreatedAtAction_WhenGroupIsValid()
        {
            // Arrange
            var validGroup = new GroupEF { Id = 1, Members = new List<UserGroupEF> { new UserGroupEF() } };
            _mockGroupService.Setup(service => service.CreateGroupAsync(validGroup)).ReturnsAsync(validGroup);

            // Act
            var result = await _controller.CreateGroup(validGroup);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(GroupController.GetGroupById), createdResult.ActionName);
            Assert.Equal(validGroup.Id, ((GroupEF)createdResult.Value).Id);
        }

        [Fact]
        public async Task GetGroupById_ReturnsNotFound_WhenGroupDoesNotExist()
        {
            // Arrange
            int nonExistentGroupId = 1;
            _mockGroupService.Setup(service => service.GetGroupByIdAsync(nonExistentGroupId)).ReturnsAsync((GroupEF)null);

            // Act
            var result = await _controller.GetGroupById(nonExistentGroupId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetGroupById_ReturnsOk_WhenGroupExists()
        {
            // Arrange
            int groupId = 1;
            var group = new GroupEF { Id = groupId };
            _mockGroupService.Setup(service => service.GetGroupByIdAsync(groupId)).ReturnsAsync(group);

            // Act
            var result = await _controller.GetGroupById(groupId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(group, okResult.Value);
        }

        [Fact]
        public async Task GetGroupsByUserId_ReturnsNotFound_WhenNoGroupsFound()
        {
            // Arrange
            int userId = 1;
            _mockGroupService.Setup(service => service.GetGroupsByUserIdAsync(userId)).ReturnsAsync((List<GroupEF>)null);

            // Act
            var result = await _controller.GetGroupsByUserId(userId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<GroupEF>>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetGroupsByUserId_ReturnsOk_WhenGroupsFound()
        {
            // Arrange
            int userId = 1;
            var groups = new List<GroupEF> { new GroupEF { Id = 1 }, new GroupEF { Id = 2 } };
            _mockGroupService.Setup(service => service.GetGroupsByUserIdAsync(userId)).ReturnsAsync(groups);

            // Act
            var result = await _controller.GetGroupsByUserId(userId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<GroupEF>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(groups, okResult.Value);
        }

        [Fact]
        public async Task GetAllGroups_ReturnsOk_WithAllGroups()
        {
            // Arrange
            var groups = new List<GroupEF> { new GroupEF { Id = 1 }, new GroupEF { Id = 2 } };
            _mockGroupService.Setup(service => service.GetAllGroupsAsync()).ReturnsAsync(groups);

            // Act
            var result = await _controller.GetAllGroups();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(groups, okResult.Value);
        }

        [Fact]
        public async Task DeleteGroup_ReturnsOk()
        {
            // Arrange
            int groupId = 1;
            _mockGroupService.Setup(service => service.DeleteGroupAsync(groupId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteGroup(groupId);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
