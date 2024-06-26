using ExpenseSharingApp.BLL.Models;
using ExpenseSharingApp.Common.GroupDTOs;
using ExpenseSharingApp.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.BLL.IServices
{
    public interface IGroupService
    {
        //Task<GroupEF> CreateGroupAsync(GroupEF group);
        //Task<GroupEF> GetGroupByIdAsync(int id);
        //Task<List<GroupEF>> GetGroupsByUserId(int userId);

        //Task<IEnumerable<GroupEF>> GetAllGroupsAsync();
        //Task<bool> AddMemberAsync(UserGroupEF userGroup);
        //Task<GroupEF> UpdateGroupAsync(GroupEF group);
        //Task DeleteGroupAsync(int id);


        Task<GroupEF> CreateGroupAsync(CreateGroupDto groupDto);
        Task<GroupEF> GetGroupByIdAsync(int id);
        Task<List<GroupEF>> GetGroupsByUserIdAsync(int userId);
        Task<IEnumerable<GroupEF>> GetAllGroupsAsync();
        Task<GroupEF> UpdateGroupAsync(GroupEF group);
        Task DeleteGroupAsync(int id);
        Task<bool> AddMemberAsync(UserGroupEF userGroup);
    }
}
