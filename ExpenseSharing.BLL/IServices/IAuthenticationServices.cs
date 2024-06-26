using ExpenseSharingApp.BLL.Models;
using ExpenseSharingApp.Common.UserDTOs;
using ExpenseSharingApp.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.BLL.IServices
{
    public interface IAuthenticationServices
    {
        //Task<UserEF> Authenticate(string email, string password);
        //Task RegisterUser(UserEF user);

        Task<UserResponseDto> Authenticate(string email, string password);
        Task RegisterUser(UserDto userDto);
    }
}
