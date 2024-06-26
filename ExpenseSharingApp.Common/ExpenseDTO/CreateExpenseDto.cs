using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.Common.ExpenseDTO
{
    public class CreateExpenseDto
    {
        [Required]
        public int? GroupId { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public int PaidBy { get; set; } // UserId

        [Required]
        public List<ExpenseSplitDto> SplitAmong { get; set; }
    }
}
