using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.BLL.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string GroupId { get; set; } // Unique identifier
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        public ICollection<UserGroup>? Members { get; set; }
        public ICollection<Expense>? Expenses { get; set; }
    }
}
