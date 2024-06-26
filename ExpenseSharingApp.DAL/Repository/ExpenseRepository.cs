using ExpenseSharingApp.Common.ExpenseDTO;
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
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly ExpenseSharingContext _context;

        public ExpenseRepository(ExpenseSharingContext context)
        {
            _context = context;
        }

        //public async Task<ExpenseEF> GetExpenseAsync(int id)
        //{
        //    return await _context.Expenses
        //        .Include(e => e.SplitAmong)
        //        .FirstOrDefaultAsync(e => e.Id == id);
        //}

        //public async Task<ExpenseEF> CreateExpenseAsync(CreateExpenseDto expenseDto)
        //{
        //    var expense = new ExpenseEF
        //    {
        //        GroupId = expenseDto.GroupId,
        //        Description = expenseDto.Description,
        //        Amount = expenseDto.Amount,
        //        PaidBy = expenseDto.PaidBy,
        //        Date = DateTime.UtcNow,
        //        SplitAmong = expenseDto.SplitAmong.Select(s => new ExpenseSplitEF
        //        {
        //            UserId = s.UserId,
        //            Amount = s.Amount,
        //            UserName=s.UserName
        //        }).ToList()
        //    };

        //    _context.Expenses.Add(expense);
        //    await _context.SaveChangesAsync();

        //    var expenseForBalance = await _context.Expenses
        //         .Include(e => e.SplitAmong)  // Ensure SplitAmong collection is loaded
        //         .FirstOrDefaultAsync(e => e.Id == expense.Id);

        //    if (expenseForBalance == null || expenseForBalance.SplitAmong == null || expenseForBalance.SplitAmong.Count == 0)
        //    {
        //        throw new Exception("Expense or SplitAmong data is missing or invalid.");
        //    }

        //    var amountPerUser = expenseForBalance.Amount / expenseForBalance.SplitAmong.Count;
        //    bool isPaidUserinSplit = false;
        //    // Example: iterate through SplitAmong and update balances
        //    foreach (var split in expenseForBalance.SplitAmong)
        //    {
        //        var user = await _context.Users.FindAsync(split.UserId);
        //        if (user != null)
        //        {
        //            // Adjust user balance
        //            user.Balance += amountPerUser;

        //            // Optionally, adjust for the person who paid
        //            if (expenseForBalance.PaidBy == user.Id)
        //            {
        //                isPaidUserinSplit = true;
        //                user.Balance -= expenseForBalance.Amount; // Subtract total amount from the payer
        //            }
        //        }
        //    }
        //    if (!isPaidUserinSplit)
        //    {
        //        var user = await _context.Users.FindAsync(expense.PaidBy);
        //        if (user != null)
        //        {
        //            user.Balance -= expenseForBalance.Amount;
        //        }

        //    }

        //    // Save changes to the database
        //    await _context.SaveChangesAsync();


        //    return expense;
        //}
        //public async Task<IEnumerable<ExpenseEF>> GetExpensesByGroupIdAsync(int groupId)
        //{
        //    return await _context.Expenses
        //                         .Where(e => e.GroupId == groupId)
        //                         .ToListAsync();
        //}

        public async Task<ExpenseEF> CreateExpenseAsync(CreateExpenseDto expenseDto)
        {
            var expense = new ExpenseEF
            {
                GroupId = expenseDto.GroupId,
                Description = expenseDto.Description,
                Amount = expenseDto.Amount,
                PaidBy = expenseDto.PaidBy,
                Date = DateTime.UtcNow,
                SplitAmong = expenseDto.SplitAmong.Select(s => new ExpenseSplitEF
                {
                    UserId = s.UserId,
                    Amount = s.Amount,
                    UserName = s.UserName
                }).ToList()
            };

            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            var expenseForBalance = await _context.Expenses
                 .Include(e => e.SplitAmong)
                 .FirstOrDefaultAsync(e => e.Id == expense.Id);

            if (expenseForBalance == null || expenseForBalance.SplitAmong == null || expenseForBalance.SplitAmong.Count == 0)
            {
                throw new Exception("Expense or SplitAmong data is missing or invalid.");
            }

            var amountPerUser = expenseForBalance.Amount / expenseForBalance.SplitAmong.Count;
            bool isPaidUserInSplit = false;

            foreach (var split in expenseForBalance.SplitAmong)
            {
                var user = await _context.Users.FindAsync(split.UserId);
                if (user != null)
                {
                    user.Balance += amountPerUser;

                    if (expenseForBalance.PaidBy == user.Id)
                    {
                        isPaidUserInSplit = true;
                        user.Balance -= expenseForBalance.Amount;
                    }
                }
            }

            if (!isPaidUserInSplit)
            {
                var user = await _context.Users.FindAsync(expense.PaidBy);
                if (user != null)
                {
                    user.Balance -= expenseForBalance.Amount;
                }
            }

            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<ExpenseEF> GetExpenseAsync(int id)
        {
            return await _context.Expenses
                .Include(e => e.SplitAmong)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<ExpenseEF>> GetExpensesByGroupIdAsync(int groupId)
        {
            return await _context.Expenses
                                 .Where(e => e.GroupId == groupId)
                                 .ToListAsync();
        }

        public async Task SettleExpenseAsync(int expenseId, int settledByUserId)
        {
            var expense = await _context.Expenses
                .Include(e => e.SplitAmong)
                .FirstOrDefaultAsync(e => e.Id == expenseId);

            if (expense == null || expense.IsSetteled)
            {
                throw new Exception("Expense not found or already settled.");
            }

            expense.IsSetteled = true;
            var paidByUser = await _context.Users.FindAsync(expense.PaidBy);

            foreach (var split in expense.SplitAmong)
            {
                var user = await _context.Users.FindAsync(split.UserId);

                if (user.Id != expense.PaidBy)
                {
                    user.Balance -= split.Amount;
                    paidByUser.Balance += split.Amount;
                }
            }

            var settlement = new ExpenseSettlementEF
            {
                GroupId = expense.GroupId ?? 0,
                ExpenseId = expense.Id,
                SettledBy = settledByUserId,
                SettledDate = DateTime.Now,
                Amount = expense.Amount
            };

            _context.ExpenseSettlements.Add(settlement);
            await _context.SaveChangesAsync();
        }
    }
}
