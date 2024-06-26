using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.BLL.Models
{
    public class UserGroup
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public string Role { get; set; } // Role within the group, e.g., "Admin" or "Normal"
    }
}
