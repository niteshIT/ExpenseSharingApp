using ExpenseSharingApp.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.DAL.IRepository
{
    public interface IUserRepository
    {
        Task<UserEF> GetUser(int userId);
        Task<IEnumerable<UserEF>> GetAllUsers();
        Task<IEnumerable<UserEF>> GetUsersByGroupIdAsync(int groupId);
    }
}
