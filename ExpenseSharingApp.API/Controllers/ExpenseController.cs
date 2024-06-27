using ExpenseSharingApp.BLL.IServices;
using ExpenseSharingApp.BLL.Models;
using ExpenseSharingApp.BLL.Services;
using ExpenseSharingApp.Common.ExpenseDTO;
using ExpenseSharingApp.DAL.Data;
using ExpenseSharingApp.DAL.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExpenseSharingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        readonly ExpenseSharingContext _context;
        public ExpenseController(IExpenseService expenseService,ExpenseSharingContext context)
        {

            _expenseService = expenseService;
            _context = context;
            
        }
        
        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto expense)
        {
            var createdExpense = await _expenseService.CreateExpenseAsync(expense);
            return CreatedAtAction(nameof(GetExpense), new { id = createdExpense.Id }, createdExpense);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> GetExpense(int id)
        {
            var expense = await _expenseService.GetExpenseAsync(id);

            if (expense == null)
            {
                return NotFound();
            }

            return Ok(expense);
        }

        [HttpGet("by-group/{groupId}")]
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult<IEnumerable<ExpenseEF>>> GetExpensesByGroupId(int groupId)
        {
            var expenses = await _expenseService.GetExpensesByGroupIdAsync(groupId);

            if (expenses == null || !expenses.Any())
            {
                return NotFound();
            }

            return Ok(expenses);
        }

        [HttpPost("settle/{expenseId}")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> SettleExpense(int expenseId, [FromBody] SettleExpenseRequest request)
        {
            if (expenseId <= 0 || request.SettledByUserId <= 0)
            {
                return BadRequest("Invalid expense ID or user ID.");
            }

            await _expenseService.SettleExpenseAsync(expenseId, request);
            return Ok();
        }

        //[HttpGet("byExpenseId/{expenseId}")]
        ////[Authorize(Roles = "admin, user")]
        //public async Task<IActionResult> GetExpenseSettlementsByExpenseId(int expenseId)
        //{
        //    var expenseSettlements = await _expenseService.GetExpenseSettlementsByExpenseIdAsync(expenseId);
        //    if (expenseSettlements == null || !expenseSettlements.Any())
        //    {
        //        return NotFound();
        //    }

        //    return Ok(expenseSettlements);
        //}

    }

}
