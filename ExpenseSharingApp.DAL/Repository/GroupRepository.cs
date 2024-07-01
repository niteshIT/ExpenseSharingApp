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
    public class GroupRepository : IGroupRepository
    {
        private readonly ExpenseSharingContext _context;

        public GroupRepository(ExpenseSharingContext context)
        {
            _context = context;
        }
        public async Task<List<GroupEF>> GetGroupsByUserId(int userId)
        {
            var groups = await _context.UserGroups
                .Where(gu => gu.UserId == userId)
                .Select(gu => gu.Group)
                .ToListAsync();

            return groups;
        }
        public async Task<GroupEF> CreateGroupAsync(GroupEF group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<GroupEF> GetGroupByIdAsync(int id)
        {
            return await _context.Groups
                .Include(g => g.Members)
                .ThenInclude(ug => ug.User)
                .FirstOrDefaultAsync(g => g.Id == id);
        }



        public async Task<IEnumerable<GroupEF>> GetAllGroupsAsync()
        {
            return await _context.Groups
                .Include(g => g.Members)
                .ThenInclude(ug => ug.User)
                .ToListAsync();
        }

        

        public async Task DeleteGroupAsync(int groupId)
        {

            var group = await _context.Groups
                .Include(g => g.Expenses)
                .ThenInclude(e => e.SplitAmong)
                .FirstOrDefaultAsync(g => g.Id == groupId);
            
            
            if (group != null)
            {
                
                foreach (var expense in group.Expenses)
                {
                    if (expense.IsSetteled == false)
                    {
                        RecalculateUserBalances(expense);
                    }

                }

                
                _context.Groups.Remove(group);
 
                
            }
            var expenseSettlements = _context.ExpenseSettlements
                .Where(es => es.GroupId == groupId)
                .ToList();
            if (expenseSettlements != null)
            {
                _context.ExpenseSettlements.RemoveRange(expenseSettlements);

            }
            await _context.SaveChangesAsync();

        }
        
        private void RecalculateUserBalances(ExpenseEF expense)
        {
            //Logic to recalculate user balances based on the deleted expense
            // This could include resetting user balances or making any necessary adjustments

            var paidByUser = _context.Users.Find(expense.PaidBy);
            foreach (var split in expense.SplitAmong)
            {

                var user = _context.Users.Find(split.UserId);
                if (user != null && user.Id != expense.PaidBy)
                {
                    user.Balance -= split.Amount;
                    paidByUser.Balance += split.Amount;
                }

            }






        }




    }
}
