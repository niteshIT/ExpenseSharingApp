using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.DAL.EF
{
    public class ExpenseSettlementEF
    {
        [Key]
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int ExpenseId { get; set; }
        public int SettledBy { get; set; }
        public DateTime SettledDate { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
}
