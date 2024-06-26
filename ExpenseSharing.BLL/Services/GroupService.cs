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

        //public async Task<GroupEF> CreateGroupAsync(GroupEF group)
        //{
        //    var groupEF = new GroupEF
        //    {
        //        //GroupId = group.GroupId,
        //        Name = group.Name,
        //        Description = group.Description,
        //        CreatedDate = group.CreatedDate,
        //        Members = group.Members.Select(m => new UserGroupEF { UserId = m.UserId, Role = m.Role }).ToList()
        //    };

        //    var createdGroupEF = await _groupRepository.CreateGroupAsync(groupEF);
        //    return new GroupEF
        //    {
        //        Id = createdGroupEF.Id,
        //        //GroupId = createdGroupEF.GroupId,
        //        Name = createdGroupEF.Name,
        //        Description = createdGroupEF.Description,
        //        CreatedDate = createdGroupEF.CreatedDate,
        //        Members = createdGroupEF.Members.Select(m => new UserGroupEF { UserId = (int)m.UserId, Role = m.Role }).ToList()
        //    };
        //}

        //public async Task<GroupEF> GetGroupByIdAsync(int id)
        //{
        //    var groupEF = await _groupRepository.GetGroupByIdAsync(id);
        //    if (groupEF == null) return null;

        //    return new GroupEF
        //    {
        //        Id = groupEF.Id,
        //        //GroupId = groupEF.GroupId,
        //        Name = groupEF.Name,
        //        Description = groupEF.Description,
        //        CreatedDate = groupEF.CreatedDate,
        //        Members = groupEF.Members.Select(m => new UserGroupEF { UserId = m.UserId, Role = m.Role }).ToList()
        //    };
        //}
        //public async Task<List<GroupEF>> GetGroupsByUserId(int userId)
        //{
        //    var groupEF = await _groupRepository.GetGroupsByUserId(userId);
        //    if (groupEF == null) return null;

        //    return groupEF;
        //}

        //public async Task<IEnumerable<GroupEF>> GetAllGroupsAsync()
        //{
        //    var groupsEF = await _groupRepository.GetAllGroupsAsync();
        //    return groupsEF.Select(g => new GroupEF
        //    {
        //        Id = g.Id,
        //        //GroupId = g.GroupId,
        //        Name = g.Name,
        //        Description = g.Description,
        //        CreatedDate = g.CreatedDate,
        //        Members = g.Members.Select(m => new UserGroupEF { UserId = m.UserId, Role = m.Role }).ToList()
        //    }).ToList();
        //}

        //public async Task<bool> AddMemberAsync(UserGroupEF userGroup)
        //{
        //    var userGroupEF = new UserGroupEF
        //    {
        //        UserId = userGroup.UserId,
        //        GroupId = userGroup.GroupId,
        //        Role = userGroup.Role
        //    };

        //    return await _groupRepository.AddMemberAsync(userGroupEF);
        //}

        //public async Task<GroupEF> UpdateGroupAsync(GroupEF group)
        //{
        //    var groupEF = new GroupEF
        //    {
        //        Id = group.Id,
        //        //GroupId = group.GroupId,
        //        Name = group.Name,
        //        Description = group.Description,
        //        CreatedDate = group.CreatedDate,
        //        Members = group.Members.Select(m => new UserGroupEF { UserId = m.UserId, Role = m.Role }).ToList()
        //    };

        //    var updatedGroupEF = await _groupRepository.UpdateGroupAsync(groupEF);
        //    return new GroupEF
        //    {
        //        Id = updatedGroupEF.Id,
        //        //GroupId = updatedGroupEF.GroupId,
        //        Name = updatedGroupEF.Name,
        //        Description = updatedGroupEF.Description,
        //        CreatedDate = updatedGroupEF.CreatedDate,
        //        Members = updatedGroupEF.Members.Select(m => new UserGroupEF { UserId = m.UserId, Role = m.Role }).ToList()
        //    };
        //}

        //public async Task DeleteGroupAsync(int id)
        //{
        //    await _groupRepository.DeleteGroupAsync(id);
        //}

        public async Task<GroupEF> CreateGroupAsync(CreateGroupDto groupDto)
        {
            var groupEF = new GroupEF
            {
                Name = groupDto.Name,
                Description = groupDto.Description,
                CreatedDate = DateTime.UtcNow,
                Members = groupDto.Members.Select(m => new UserGroupEF { UserId = m.UserId, Role = m.Role }).ToList()
            };

            return await _groupRepository.CreateGroupAsync(groupEF);
        }

        public async Task<GroupEF> GetGroupByIdAsync(int id)
        {
            return await _groupRepository.GetGroupByIdAsync(id);
        }

        public async Task<List<GroupEF>> GetGroupsByUserIdAsync(int userId)
        {
            return await _groupRepository.GetGroupsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<GroupEF>> GetAllGroupsAsync()
        {
            return await _groupRepository.GetAllGroupsAsync();
        }

        public async Task<GroupEF> UpdateGroupAsync(GroupEF group)
        {
            return await _groupRepository.UpdateGroupAsync(group);
        }

        public async Task DeleteGroupAsync(int id)
        {
            await _groupRepository.DeleteGroupAsync(id);
        }

        public async Task<bool> AddMemberAsync(UserGroupEF userGroup)
        {
            return await _groupRepository.AddMemberAsync(userGroup);
        }
    }
}
