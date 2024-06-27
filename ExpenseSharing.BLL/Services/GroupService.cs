using ExpenseSharingApp.BLL.IServices;
using ExpenseSharingApp.BLL.Models;
using ExpenseSharingApp.Common.GroupDTOs;
using ExpenseSharingApp.DAL.EF;
using ExpenseSharingApp.DAL.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.BLL.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task<GroupEF> CreateGroupAsync(GroupEF group)
        {
            var groupEF = new GroupEF
            {
                Name = group.Name,
                Description = group.Description,
                CreatedDate = DateTime.UtcNow,
                Members = group.Members.Select(m => new UserGroupEF { UserId = m.UserId, Role = m.Role }).ToList()
            };

            var createdGroupEF = await _groupRepository.CreateGroupAsync(groupEF);
            return MapToGroupEF(createdGroupEF);
        }

        public async Task<GroupEF> GetGroupByIdAsync(int id)
        {
            var groupEF = await _groupRepository.GetGroupByIdAsync(id);
            if (groupEF == null) return null;

            return MapToGroupEF(groupEF);
        }

        public async Task<List<GroupEF>> GetGroupsByUserIdAsync(int userId)
        {
            var groupsEF = await _groupRepository.GetGroupsByUserId(userId);
            return groupsEF;
        }

        public async Task<IEnumerable<GroupEF>> GetAllGroupsAsync()
        {
            var groupsEF = await _groupRepository.GetAllGroupsAsync();
            return groupsEF.Select(MapToGroupEF).ToList();
        }

        public async Task DeleteGroupAsync(int id)
        {
            await _groupRepository.DeleteGroupAsync(id);
        }

        private GroupEF MapToGroupEF(GroupEF groupEF)
        {
            return new GroupEF
            {
                Id = groupEF.Id,
                Name = groupEF.Name,
                Description = groupEF.Description,
                CreatedDate = groupEF.CreatedDate,
                Members = groupEF.Members.Select(m => new UserGroupEF { UserId = m.UserId, Role = m.Role }).ToList()
            };
        }

       

    }
}
