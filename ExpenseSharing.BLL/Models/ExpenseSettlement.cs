using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.BLL.Models
{
    public class ExpenseSettlement
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int ExpenseId { get; set; }
        public Expense Expense { get; set; }
        public int SettledBy { get; set; } // UserId
        public User User { get; set; }
        public DateTime SettledDate { get; set; }
        public decimal Amount { get; set; }
    }
}
