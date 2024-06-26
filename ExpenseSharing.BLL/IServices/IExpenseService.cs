﻿using ExpenseSharingApp.BLL.Models;
using ExpenseSharingApp.Common.ExpenseDTO;
using ExpenseSharingApp.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.BLL.IServices
{
    public interface IExpenseService
    {
        //Task<ExpenseEF> GetExpenseAsync(int id);
        //Task<ExpenseEF> CreateExpenseAsync(CreateExpenseDto expense);
        //Task<IEnumerable<ExpenseEF>> GetExpensesByGroupIdAsync(int groupId);
        //Task SettleExpenseAsync(int expenseId, int settledByUserId);


        Task<ExpenseEF> CreateExpenseAsync(CreateExpenseDto expenseDto);
        Task<ExpenseEF> GetExpenseAsync(int id);
        Task<IEnumerable<ExpenseEF>> GetExpensesByGroupIdAsync(int groupId);
        Task SettleExpenseAsync(int expenseId, SettleExpenseRequest request);
    }
}
