using ExpenseSharingApp.DAL.Data;
using ExpenseSharingApp.DAL.EF;
using ExpenseSharingApp.DAL.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.DAL.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ExpenseSharingContext _context;

        public GroupRepository(ExpenseSharingContext context)
        {
            _context = context;
        }
        public async Task<List<GroupEF>> GetGroupsByUserId(int userId)
        {
            var groups = await _context.UserGroups
                .Where(gu => gu.UserId == userId)
                .Select(gu => gu.Group)
                .ToListAsync();

            return groups;
        }
        public async Task<GroupEF> CreateGroupAsync(GroupEF group)
        {
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();
            return group;
        }

        public async Task<GroupEF> GetGroupByIdAsync(int id)
        {
            return await _context.Groups
                .Include(g => g.Members)
                .ThenInclude(ug => ug.User)
                .FirstOrDefaultAsync(g => g.Id == id);
        }



        public async Task<IEnumerable<GroupEF>> GetAllGroupsAsync()
        {
            return await _context.Groups
                .Include(g => g.Members)
                .ThenInclude(ug => ug.User)
                .ToListAsync();
        }

        

        public async Task DeleteGroupAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group != null)
            {
                _context.Groups.Remove(group);
                await _context.SaveChangesAsync();
            }
        }

        
    }
}
