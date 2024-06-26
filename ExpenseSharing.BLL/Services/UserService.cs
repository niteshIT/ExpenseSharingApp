using ExpenseSharingApp.BLL.IServices;
using ExpenseSharingApp.BLL.Models;
using ExpenseSharingApp.DAL.EF;
using ExpenseSharingApp.DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.BLL.Services
{
    public class UserService:IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserEF> GetUser(int userId)
        {
            // Additional business logic if needed
            return await _userRepository.GetUser(userId);
        }

        public async Task<IEnumerable<UserEF>> GetAllUsers()
        {
            // Additional business logic if needed
            return await _userRepository.GetAllUsers();
        }
    }
}
