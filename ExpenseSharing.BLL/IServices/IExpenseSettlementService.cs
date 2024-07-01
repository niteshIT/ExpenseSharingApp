﻿using ExpenseSharingApp.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.BLL.IServices
{
    public interface IExpenseSettlementService
    {
        Task<ExpenseSettlementEF> GetExpenseSettlementByIdAsync(int id);
        Task<List<ExpenseSettlementEF>> GetExpenseSettlementsAsync();
        Task<List<ExpenseSettlementEF>> GetExpenseSettlementsByGroupIdAsync(int groupId);
        Task AddExpenseSettlementAsync(ExpenseSettlementEF expenseSettlement);
        Task UpdateExpenseSettlementAsync(ExpenseSettlementEF expenseSettlement);
        Task DeleteExpenseSettlementAsync(int id);
    }

}
