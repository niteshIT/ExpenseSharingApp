using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.DAL.EF
{
    public class UserEF
    {
        [Key]
        public int Id { get; set; }
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public string? Role { get; set; } = "user"; // Admin or Normal

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0;


        // Navigation properties
        public ICollection<UserGroupEF>? UserGroups { get; set; }
        public string? Token { get; set; }
    }
}
