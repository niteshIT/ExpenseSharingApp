using ExpenseSharingApp.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.BLL.IServices
{
    public interface IUserService
    {
        Task<UserEF> GetUser(int userId);
        Task<IEnumerable<UserEF>> GetAllUsers();
    }
}
