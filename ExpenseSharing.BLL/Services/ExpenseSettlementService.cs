using ExpenseSharingApp.BLL.IServices;
using ExpenseSharingApp.DAL.EF;
using ExpenseSharingApp.DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.BLL.Services
{
    public class ExpenseSettlementService : IExpenseSettlementService
    {
        private readonly IExpenseSettlementRepository _repository;

        public ExpenseSettlementService(IExpenseSettlementRepository repository)
        {
            _repository = repository;
        }

        public Task<ExpenseSettlementEF> GetExpenseSettlementByIdAsync(int id)
        {
            return _repository.GetExpenseSettlementByIdAsync(id);
        }

        public Task<List<ExpenseSettlementEF>> GetExpenseSettlementsAsync()
        {
            return _repository.GetExpenseSettlementsAsync();
        }

        public Task<List<ExpenseSettlementEF>> GetExpenseSettlementsByGroupIdAsync(int groupId)
        {
            return _repository.GetExpenseSettlementsByGroupIdAsync(groupId);
        }

        public Task AddExpenseSettlementAsync(ExpenseSettlementEF expenseSettlement)
        {
            return _repository.AddExpenseSettlementAsync(expenseSettlement);
        }

        public Task UpdateExpenseSettlementAsync(ExpenseSettlementEF expenseSettlement)
        {
            return _repository.UpdateExpenseSettlementAsync(expenseSettlement);
        }

        public Task DeleteExpenseSettlementAsync(int id)
        {
            return _repository.DeleteExpenseSettlementAsync(id);
        }
    }

}
