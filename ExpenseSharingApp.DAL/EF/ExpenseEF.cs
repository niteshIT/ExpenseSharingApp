using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace ExpenseSharingApp.DAL.EF
{
    public class ExpenseEF
    {
        [Key]
        public int Id { get; set; }

       
        public int? GroupId { get; set; }

        public string? Description { get; set; }
  
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public int? PaidBy { get; set; } // UserId

        public DateTime? Date { get; set; }
        public bool IsSetteled { get; set; } = false;

        // Navigation properties
        public GroupEF? Group { get; set; }

        public ICollection<ExpenseSplitEF>? SplitAmong { get; set; }
    }
}
