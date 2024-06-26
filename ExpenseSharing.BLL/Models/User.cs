using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.BLL.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; } // Can be "Admin" or "Normal"
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; }
        public string? Token { get; set; }
    }
}
