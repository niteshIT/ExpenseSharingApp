﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.Common.ExpenseDTO
{
    public class ExpenseSplitDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
