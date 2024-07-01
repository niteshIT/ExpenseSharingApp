using ExpenseSharingApp.BLL.IServices;
using ExpenseSharingApp.BLL.Models;
using ExpenseSharingApp.BLL.Services;
using ExpenseSharingApp.Common.GroupDTOs;
using ExpenseSharingApp.DAL.Data;
using ExpenseSharingApp.DAL.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExpenseSharingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IExpenseSettlementService _expenseSettlementService;

        public GroupController(IGroupService groupService, IExpenseSettlementService expenseSettlementService)
        {
            _groupService = groupService;
           
            _expenseSettlementService= expenseSettlementService;
        }

        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> CreateGroup([FromBody] GroupEF group)
        {
            if (group == null || group.Members == null || group.Members.Count > 10)
            {
                return BadRequest("Invalid group data.");
            }

            var createdGroup = await _groupService.CreateGroupAsync(group);
            return CreatedAtAction(nameof(GetGroupById), new { id = createdGroup.Id }, createdGroup);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> GetGroupById(int id)
        {
            var group = await _groupService.GetGroupByIdAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            return Ok(group);
        }


        [HttpGet("user/{userId}")]
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult<List<GroupEF>>> GetGroupsByUserId(int userId)
        {
            var groups = await _groupService.GetGroupsByUserIdAsync(userId);
            if (groups == null || !groups.Any())
            {
                return NotFound();
            }

            return Ok(groups);
        }


        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> GetAllGroups()
        {
            var groups = await _groupService.GetAllGroupsAsync();
            return Ok(groups);
        }
        [HttpDelete("{groupId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            

            await _groupService.DeleteGroupAsync(groupId);

            return Ok();
        }




            //[HttpDelete("{groupId}")]
            //[Authorize(Roles = "admin")]
            //public async Task<IActionResult> DeleteGroup(int groupId)
            //{
            //    var group = await _context.Groups
            //        .Include(g => g.Expenses)
            //        .ThenInclude(e => e.SplitAmong)
            //        .FirstOrDefaultAsync(g => g.Id == groupId);
            //    var expenseSettlements = _context.ExpenseSettlements
            //        .Where(es => es.GroupId == groupId)
            //        .ToList();




            //    if (group == null)
            //    {
            //        return NotFound("Group not found.");
            //    }

            //    Recalculate balances before deleting the group
            //    foreach (var expense in group.Expenses)
            //    {
            //        if (expense.IsSetteled == false)
            //        {
            //            RecalculateUserBalances(expense);
            //        }

            //    }
            //    if (expenseSettlements != null)
            //    {
            //        _context.ExpenseSettlements.RemoveRange(expenseSettlements);

            //    }
            //    _context.Groups.Remove(group);
            //    await _context.SaveChangesAsync();

            //    return Ok();
            //}

            //private void RecalculateUserBalances(ExpenseEF expense)
            //{
            //    Logic to recalculate user balances based on the deleted expense
            //     This could include resetting user balances or making any necessary adjustments

            //    var paidByUser = _context.Users.Find(expense.PaidBy);
            //    foreach (var split in expense.SplitAmong)
            //    {

            //        var user = _context.Users.Find(split.UserId);
            //        if (user != null && user.Id != expense.PaidBy)
            //        {
            //            user.Balance -= split.Amount;
            //            paidByUser.Balance += split.Amount;
            //        }

            //    }






            //}




        }
}
