using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.BLL.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public int PaidBy { get; set; } // UserId
        public User User { get; set; }
        public DateTime Date { get; set; }

        public ICollection<ExpenseSplit>? SplitAmong { get; set; }
    }
}
