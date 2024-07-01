using ExpenseSharingApp.Common.UserDTOs;
using ExpenseSharingApp.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.DAL.IRepository
{
    public interface IAuthenticationRepository
    {
      
        Task<UserEF> Authenticate(string email, string password);
        Task RegisterUser(UserDto userDto);
        //Task<UserEF> GetUserByEmail(string email);
    }
}
