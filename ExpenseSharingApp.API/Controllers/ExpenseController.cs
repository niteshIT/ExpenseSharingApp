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
        //    [HttpPost]
        //    //[Authorize(Roles = "admin, user")]
        //    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto expense)
        //    {


        //            var createdExpense = await _expenseService.CreateExpenseAsync(expense);
        //            return CreatedAtAction(nameof(GetExpense), new { id = createdExpense.Id }, createdExpense);

        //    }

        //    [HttpGet("{id}")]
        //    //[Authorize(Roles = "admin, user")]
        //    public async Task<IActionResult> GetExpense(int id)
        //    {
        //        var expense = await _expenseService.GetExpenseAsync(id);

        //        if (expense == null)
        //        {
        //            return NotFound();
        //        }

        //        return Ok(expense);
        //    }
        //    [HttpGet("by-group/{groupId}")]
        //    //[Authorize(Roles = "admin, user")]
        //    public async Task<ActionResult<IEnumerable<ExpenseEF>>> GetExpensesByGroupId(int groupId)
        //    {
        //        var expenses = await _expenseService.GetExpensesByGroupIdAsync(groupId);

        //        if (expenses == null || !expenses.Any())
        //        {
        //            return NotFound();
        //        }

        //        return Ok(expenses);
        //    }

        //    [HttpPost("settle/{expenseId}")]
        //    //[Authorize(Roles = "admin, user")]
        //    public async Task<IActionResult> SettleExpense(int expenseId, [FromBody] SettleExpenseRequest request)
        //    {

        //        if (expenseId <= 0 || request.SettledByUserId <= 0)
        //        {
        //            return BadRequest("Invalid expense ID or user ID.");
        //        }
        //        var expense = await _context.Expenses
        //            .Include(e => e.SplitAmong)
        //            .FirstOrDefaultAsync(e => e.Id == expenseId);

        //        if (expense == null)
        //        {
        //            return NotFound();
        //        }

        //        if (expense.IsSetteled)
        //        {
        //            return BadRequest("Expense is already settled.");
        //        }

        //        expense.IsSetteled = true;

        //        // Update balances
        //        var paidByUser = await _context.Users.FindAsync(expense.PaidBy);

        //        foreach (var split in expense.SplitAmong)
        //        {
        //            var user = await _context.Users.FindAsync(split.UserId);

        //            if (user.Id != expense.PaidBy)
        //            {
        //                // Decrease the debt for the user who owes money
        //                user.Balance -= split.Amount;

        //                // Increase the balance for the user who paid
        //                paidByUser.Balance += split.Amount;
        //            }
        //        }

        //        var settlement = new ExpenseSettlementEF
        //        {
        //            GroupId = expense.GroupId ?? 0,
        //            ExpenseId = expense.Id,
        //            SettledBy = request.SettledByUserId,
        //            SettledDate = DateTime.Now,
        //            Amount = expense.Amount
        //        };

        //        _context.ExpenseSettlements.Add(settlement);

        //        await _context.SaveChangesAsync();
        //        return Ok( expense);
        //    }
        //    private bool ExpenseExists(int id)
        //    {
        //        return _context.Expenses.Any(e => e.Id == id);
        //    }


        [HttpPost]
        //[Authorize(Roles = "admin, user")]
        public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseDto expense)
        {
            var createdExpense = await _expenseService.CreateExpenseAsync(expense);
            return CreatedAtAction(nameof(GetExpense), new { id = createdExpense.Id }, createdExpense);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "admin, user")]
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
        //[Authorize(Roles = "admin, user")]
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
        //[Authorize(Roles = "admin, user")]
        public async Task<IActionResult> SettleExpense(int expenseId, [FromBody] SettleExpenseRequest request)
        {
            if (expenseId <= 0 || request.SettledByUserId <= 0)
            {
                return BadRequest("Invalid expense ID or user ID.");
            }

            await _expenseService.SettleExpenseAsync(expenseId, request);
            return Ok();
        }
    }

}
