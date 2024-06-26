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
    public class ExpenseSplitEF
    {
        [Key]
        public int Id { get; set; }

       
        public int? ExpenseId { get; set; }
        public string? UserName { get; set; }

        public int? UserId { get; set; }

        
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        // Navigation properties
        [JsonIgnore]
        public ExpenseEF? Expense { get; set; }
        public UserEF? User { get; set; }
    }
}
