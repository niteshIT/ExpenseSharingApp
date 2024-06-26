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
    public class UserRepository : IUserRepository
    {
        private readonly ExpenseSharingContext _appDbContext;
        public UserRepository(ExpenseSharingContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IEnumerable<UserEF>> GetAllUsers()
        {
            return await _appDbContext.Users.ToListAsync();
        }

        public async Task<UserEF> GetUser(int userId)
        {
            return await _appDbContext.Users.FindAsync(userId);
        }

       
    }
}
