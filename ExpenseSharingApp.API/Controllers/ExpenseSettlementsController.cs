using ExpenseSharingApp.BLL.IServices;
using ExpenseSharingApp.DAL.Data;
using ExpenseSharingApp.DAL.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExpenseSharingApp.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public class ExpenseSettlementsController : ControllerBase
    //{
    //    readonly ExpenseSharingContext _context;
    //    public ExpenseSettlementsController(ExpenseSharingContext expenseSharingContext)
    //    {
    //        _context = expenseSharingContext;

    //    }
    //    //POST: api/ExpenseSettlements
    //    [HttpPost]
    //    [Authorize(Roles = "admin, user")]
    //    public async Task<ActionResult<ExpenseSettlementEF>> PostExpenseSettlement(ExpenseSettlementEF expenseSettlement)
    //    {
    //        _context.ExpenseSettlements.Add(expenseSettlement);
    //        await _context.SaveChangesAsync();

    //        return CreatedAtAction("GetExpenseSettlement", new { id = expenseSettlement.Id }, expenseSettlement);
    //    }
    //    // GET: api/ExpenseSettlements/5
    //    [HttpGet("{id}")]
    //    [Authorize(Roles = "admin, user")]
    //    public async Task<ActionResult<ExpenseSettlementEF>> GetExpenseSettlement(int id)
    //    {
    //        var expenseSettlement = await _context.ExpenseSettlements.FindAsync(id);

    //        if (expenseSettlement == null)
    //        {
    //            return NotFound();
    //        }

    //        return expenseSettlement;
    //    }
    //    [HttpGet("byExpenseId/{id}")]
    //    [Authorize(Roles = "admin, user")]
    //    public async Task<ActionResult<ExpenseSettlementEF>> GetExpenseSettlementbyExpenseId(int id)
    //    {
    //        var expenseSettlement = await _context.ExpenseSettlements
    //                                              .SingleOrDefaultAsync(s => s.ExpenseId == id);

    //        if (expenseSettlement == null)
    //        {
    //            return NotFound();
    //        }

    //        return Ok(expenseSettlement);
    //    }
    //    [HttpGet]
    //    [Authorize(Roles = "admin, user")]
    //    public async Task<ActionResult<IEnumerable<ExpenseSettlementEF>>> GetExpenseSettlements()
    //    {
    //        return await _context.ExpenseSettlements.ToListAsync();
    //    }
    //    [HttpPut("{id}")]
    //    [Authorize(Roles = "admin, user")]
    //    public async Task<IActionResult> PutExpenseSettlement(int id, ExpenseSettlementEF expenseSettlement)
    //    {
    //        if (id != expenseSettlement.Id)
    //        {
    //            return BadRequest();
    //        }

    //        _context.Entry(expenseSettlement).State = EntityState.Modified;

    //        try
    //        {
    //            await _context.SaveChangesAsync();
    //        }
    //        catch (DbUpdateConcurrencyException)
    //        {
    //            if (!ExpenseSettlementExists(id))
    //            {
    //                return NotFound();
    //            }
    //            else
    //            {
    //                throw;
    //            }
    //        }

    //        return NoContent();
    //    }



    //    // DELETE: api/ExpenseSettlements/5
    //    [HttpDelete("{id}")]
    //    [Authorize(Roles = "admin, user")]
    //    public async Task<IActionResult> DeleteExpenseSettlement(int id)
    //    {
    //        var expenseSettlement = await _context.ExpenseSettlements.FindAsync(id);
    //        if (expenseSettlement == null)
    //        {
    //            return NotFound();
    //        }

    //        _context.ExpenseSettlements.Remove(expenseSettlement);
    //        await _context.SaveChangesAsync();

    //        return NoContent();
    //    }

    //    private bool ExpenseSettlementExists(int id)
    //    {
    //        return _context.ExpenseSettlements.Any(e => e.Id == id);
    //    }

    //    [HttpGet("byGroupId/{groupId}")]
    //    public async Task<ActionResult<IEnumerable<ExpenseSettlementEF>>> GetExpenseSettlementsByGroupId(int groupId)
    //    {
    //        var expenseSettlements = await _context.ExpenseSettlements
    //                                              .Where(s => s.GroupId == groupId)
    //                                              .ToListAsync();

    //        if (!expenseSettlements.Any())
    //        {
    //            return NotFound();
    //        }

    //        return expenseSettlements;
    //    }

    //}

    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseSettlementsController : ControllerBase
    {
        private readonly IExpenseSettlementService _expenseSettlementService;

        public ExpenseSettlementsController(IExpenseSettlementService expenseSettlementService)
        {
            _expenseSettlementService = expenseSettlementService;
        }

        // GET: api/ExpenseSettlements
        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult<IEnumerable<ExpenseSettlementEF>>> GetExpenseSettlements()
        {
            var settlements = await _expenseSettlementService.GetExpenseSettlementsAsync();
            return Ok(settlements);
        }

        // GET: api/ExpenseSettlements/5
        [HttpGet("{id}")]
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult<ExpenseSettlementEF>> GetExpenseSettlement(int id)
        {
            var settlement = await _expenseSettlementService.GetExpenseSettlementByIdAsync(id);

            if (settlement == null)
            {
                return NotFound();
            }

            return Ok(settlement);
        }

        // GET: api/ExpenseSettlements/byExpenseId/5
        //[HttpGet("byExpenseId/{id}")]
        //[Authorize(Roles = "admin, user")]
        //public async Task<ActionResult<ExpenseSettlementEF>> GetExpenseSettlementByExpenseId(int id)
        //{
        //    var settlement = await _expenseSettlementService.GetExpenseSettlementByExpenseIdAsync(id);

        //    if (settlement == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(settlement);
        //}

        // GET: api/ExpenseSettlements/byGroupId/5
        [HttpGet("byGroupId/{groupId}")]
        //[Authorize(Roles = "admin, user")]
        public async Task<ActionResult<IEnumerable<ExpenseSettlementEF>>> GetExpenseSettlementsByGroupId(int groupId)
        {
            var settlements = await _expenseSettlementService.GetExpenseSettlementsByGroupIdAsync(groupId);

            if (!settlements.Any())
            {
                return NotFound();
            }

            return Ok(settlements);
        }

        

        // POST: api/ExpenseSettlements
        [HttpPost]
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult<ExpenseSettlementEF>> PostExpenseSettlement(ExpenseSettlementEF expenseSettlement)
        {
            await _expenseSettlementService.AddExpenseSettlementAsync(expenseSettlement);

            return CreatedAtAction(nameof(GetExpenseSettlement), new { id = expenseSettlement.Id }, expenseSettlement);
        }

        // DELETE: api/ExpenseSettlements/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> DeleteExpenseSettlement(int id)
        {
            var settlement = await _expenseSettlementService.GetExpenseSettlementByIdAsync(id);
            if (settlement == null)
            {
                return NotFound();
            }

            await _expenseSettlementService.DeleteExpenseSettlementAsync(id);

            return NoContent();
        }

        
    }
}
