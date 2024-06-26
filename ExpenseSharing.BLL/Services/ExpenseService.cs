using ExpenseSharingApp.BLL.IServices;
using ExpenseSharingApp.BLL.Models;
using ExpenseSharingApp.Common.ExpenseDTO;
using ExpenseSharingApp.DAL.EF;
using ExpenseSharingApp.DAL.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.BLL.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        

        public ExpenseService(IExpenseRepository expenseRepository )
        {
            _expenseRepository = expenseRepository;
            
        }

        //public Task<ExpenseEF> CreateExpenseAsync(CreateExpenseDto expense)
        //{
        //    var res= _expenseRepository.CreateExpenseAsync(expense);
        //    return res;
        //}

        //public Task<ExpenseEF> GetExpenseAsync(int id)
        //{
        //    var res= _expenseRepository.GetExpenseAsync(id);
        //    return res;
        //}
        //public async Task<IEnumerable<ExpenseEF>> GetExpensesByGroupIdAsync(int groupId)
        //{
        //    var result = await _expenseRepository.GetExpensesByGroupIdAsync(groupId);
        //    return result;
        //}


        public async Task<ExpenseEF> CreateExpenseAsync(CreateExpenseDto expenseDto)
        {
            return await _expenseRepository.CreateExpenseAsync(expenseDto);
        }

        public async Task<ExpenseEF> GetExpenseAsync(int id)
        {
            return await _expenseRepository.GetExpenseAsync(id);
        }

        public async Task<IEnumerable<ExpenseEF>> GetExpensesByGroupIdAsync(int groupId)
        {
            return await _expenseRepository.GetExpensesByGroupIdAsync(groupId);
        }

        public async Task SettleExpenseAsync(int expenseId, SettleExpenseRequest request)
        {
            await _expenseRepository.SettleExpenseAsync(expenseId, request.SettledByUserId);
        }
    }
}
