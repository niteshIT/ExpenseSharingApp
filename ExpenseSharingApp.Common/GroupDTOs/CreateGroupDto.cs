using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.Common.GroupDTOs
{
    public class CreateGroupDto
    {
       
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UserGroupDto> Members { get; set; }
    }
}
