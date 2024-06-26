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
    public class UserGroupEF
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? GroupId { get; set; }

        public string? Role { get; set; } // Admin or Normal

        // Navigation properties
        [JsonIgnore]
        public UserEF? User { get; set; }
        public GroupEF? Group { get; set; }
    }
}
