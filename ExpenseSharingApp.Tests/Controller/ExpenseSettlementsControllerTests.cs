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
    public class ExpenseSettlementsControllerTests
    {
        private readonly Mock<IExpenseSettlementService> _mockExpenseSettlementService;
        private readonly ExpenseSettlementsController _controller;

        public ExpenseSettlementsControllerTests()
        {
            _mockExpenseSettlementService = new Mock<IExpenseSettlementService>();
            _controller = new ExpenseSettlementsController(_mockExpenseSettlementService.Object);
        }

        [Fact]
        public async Task GetExpenseSettlements_ReturnsOk()
        {
            // Arrange
            var expectedSettlements = new List<ExpenseSettlementEF> { new ExpenseSettlementEF { Id = 1, GroupId = 1 }, new ExpenseSettlementEF { Id = 2, GroupId = 1 } };
            _mockExpenseSettlementService.Setup(service => service.GetExpenseSettlementsAsync()).ReturnsAsync(expectedSettlements);

            // Act
            var result = await _controller.GetExpenseSettlements();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var settlements = Assert.IsAssignableFrom<IEnumerable<ExpenseSettlementEF>>(okResult.Value);
            Assert.Equal(expectedSettlements.Count, settlements.Count());
        }

        [Fact]
        public async Task GetExpenseSettlement_ReturnsOk_WhenSettlementExists()
        {
            // Arrange
            int settlementId = 1;
            var expectedSettlement = new ExpenseSettlementEF { Id = settlementId, GroupId = 1 };
            _mockExpenseSettlementService.Setup(service => service.GetExpenseSettlementByIdAsync(settlementId)).ReturnsAsync(expectedSettlement);

            // Act
            var result = await _controller.GetExpenseSettlement(settlementId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var settlement = Assert.IsType<ExpenseSettlementEF>(okResult.Value);
            Assert.Equal(expectedSettlement.Id, settlement.Id);
        }

        [Fact]
        public async Task GetExpenseSettlement_ReturnsNotFound_WhenSettlementDoesNotExist()
        {
            // Arrange
            int settlementId = 1;
            _mockExpenseSettlementService.Setup(service => service.GetExpenseSettlementByIdAsync(settlementId)).ReturnsAsync((ExpenseSettlementEF)null);

            // Act
            var result = await _controller.GetExpenseSettlement(settlementId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetExpenseSettlementsByGroupId_ReturnsOk()
        {
            // Arrange
            int groupId = 1;
            var expectedSettlements = new List<ExpenseSettlementEF> { new ExpenseSettlementEF { Id = 1, GroupId = groupId }, new ExpenseSettlementEF { Id = 2, GroupId = groupId } };
            _mockExpenseSettlementService.Setup(service => service.GetExpenseSettlementsByGroupIdAsync(groupId)).ReturnsAsync(expectedSettlements);

            // Act
            var result = await _controller.GetExpenseSettlementsByGroupId(groupId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var settlements = Assert.IsAssignableFrom<IEnumerable<ExpenseSettlementEF>>(okResult.Value);
            Assert.Equal(expectedSettlements.Count, settlements.Count());
        }

        [Fact]
        public async Task PostExpenseSettlement_ReturnsCreatedAtAction()
        {
            // Arrange
            var newSettlement = new ExpenseSettlementEF { Id = 1, GroupId = 1 };

            // Act
            var result = await _controller.PostExpenseSettlement(newSettlement);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(_controller.GetExpenseSettlement), createdAtActionResult.ActionName);
            Assert.Equal(newSettlement.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(newSettlement, createdAtActionResult.Value);
        }

        [Fact]
        public async Task DeleteExpenseSettlement_ReturnsNoContent()
        {
            // Arrange
            int settlementId = 1;
            var existingSettlement = new ExpenseSettlementEF { Id = settlementId, GroupId = 1 };
            _mockExpenseSettlementService.Setup(service => service.GetExpenseSettlementByIdAsync(settlementId)).ReturnsAsync(existingSettlement);

            // Act
            var result = await _controller.DeleteExpenseSettlement(settlementId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteExpenseSettlement_ReturnsNotFound_WhenSettlementDoesNotExist()
        {
            // Arrange
            int settlementId = 1;
            _mockExpenseSettlementService.Setup(service => service.GetExpenseSettlementByIdAsync(settlementId)).ReturnsAsync((ExpenseSettlementEF)null);

            // Act
            var result = await _controller.DeleteExpenseSettlement(settlementId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
