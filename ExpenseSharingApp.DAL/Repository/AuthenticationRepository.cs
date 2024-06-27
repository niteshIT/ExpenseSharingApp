using ExpenseSharingApp.Common.UserDTOs;
using ExpenseSharingApp.DAL.Data;
using ExpenseSharingApp.DAL.EF;
using ExpenseSharingApp.DAL.IRepository;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseSharingApp.DAL.Repository
{
    public class AuthenticationRepository:IAuthenticationRepository
    {
        private readonly ExpenseSharingContext _context;

        public AuthenticationRepository(ExpenseSharingContext appDbContext)
        {
            _context = appDbContext;
        }

  
        public async Task<UserEF> Authenticate(string email, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (user == null)
                return null;

            // Verify password hash
            if (password!= user.Password)
                return null;

            return user;
        }

        public async Task RegisterUser(UserDto userDto)
        {
            var user = new UserEF
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                Password =userDto.Password,
                
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserEF> GetUserByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        // Helper methods for hashing and verifying passwords
       
    }
}
