using ExpenseSharingApp.Common.ExpenseDTO;
using ExpenseSharingApp.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.DAL.IRepository
{
    public interface IExpenseRepository
    {
        //Task<ExpenseEF> GetExpenseAsync(int id);
        //Task<ExpenseEF> CreateExpenseAsync(CreateExpenseDto expenseDto);
        //Task<IEnumerable<ExpenseEF>> GetExpensesByGroupIdAsync(int groupId);


        Task<ExpenseEF> CreateExpenseAsync(CreateExpenseDto expenseDto);
        Task<ExpenseEF> GetExpenseAsync(int id);
        Task<IEnumerable<ExpenseEF>> GetExpensesByGroupIdAsync(int groupId);
        Task SettleExpenseAsync(int expenseId, int settledByUserId);
    }
}
