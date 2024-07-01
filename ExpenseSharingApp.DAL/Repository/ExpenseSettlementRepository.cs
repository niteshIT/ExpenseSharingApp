using ExpenseSharingApp.DAL.Data;
using ExpenseSharingApp.DAL.EF;
using ExpenseSharingApp.DAL.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.DAL.Repository
{
    public class ExpenseSettlementRepository : IExpenseSettlementRepository
    {
        private readonly ExpenseSharingContext _context;

        public ExpenseSettlementRepository(ExpenseSharingContext context)
        {
            _context = context;
        }

        public async Task<ExpenseSettlementEF> GetExpenseSettlementByIdAsync(int id)
        {
            return await _context.ExpenseSettlements.FindAsync(id);
        }

        public async Task<List<ExpenseSettlementEF>> GetExpenseSettlementsAsync()
        {
            return await _context.ExpenseSettlements.ToListAsync();
        }

        public async Task<List<ExpenseSettlementEF>> GetExpenseSettlementsByGroupIdAsync(int groupId)
        {
            return await _context.ExpenseSettlements
                .Where(es => es.GroupId == groupId)
                .ToListAsync();
        }

        public async Task AddExpenseSettlementAsync(ExpenseSettlementEF expenseSettlement)
        {
            _context.ExpenseSettlements.Add(expenseSettlement);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateExpenseSettlementAsync(ExpenseSettlementEF expenseSettlement)
        {
            _context.Entry(expenseSettlement).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteExpenseSettlementAsync(int id)
        {
            var expenseSettlement = await _context.ExpenseSettlements.FindAsync(id);
            if (expenseSettlement != null)
            {
                _context.ExpenseSettlements.Remove(expenseSettlement);
                await _context.SaveChangesAsync();
            }
        }
    }

}
