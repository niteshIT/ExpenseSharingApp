using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.DAL.EF
{
    public class GroupEF
    {
        [Key]
        public int Id { get; set; }

       
        public string? Name { get; set; }

        public string? Description { get; set; }

        
        public DateTime? CreatedDate { get; set; }

        // Navigation properties
        public ICollection<UserGroupEF>? Members { get; set; }
        public ICollection<ExpenseEF>? Expenses { get; set; }
    }
}
