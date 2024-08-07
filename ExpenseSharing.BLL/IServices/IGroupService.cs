﻿using ExpenseSharingApp.BLL.Models;
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
        Task<GroupEF> CreateGroupAsync(GroupEF group);
        Task<GroupEF> GetGroupByIdAsync(int id);
        Task<List<GroupEF>> GetGroupsByUserIdAsync(int userId);

        Task<IEnumerable<GroupEF>> GetAllGroupsAsync();
        
        Task DeleteGroupAsync(int id);



    }
}
