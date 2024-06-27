using ExpenseSharingApp.BLL.IServices;
using ExpenseSharingApp.BLL.Models;
using ExpenseSharingApp.DAL.Repository;
using ExpenseSharingApp.DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseSharingApp.DAL.EF;
using ExpenseSharingApp.Common.UserDTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ExpenseSharingApp.BLL.Services
{
    public class AuthenticationServices:IAuthenticationServices

    {
        private readonly  IAuthenticationRepository _authenticationRepository;
        public AuthenticationServices(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;

        }
       

        public async Task<UserResponseDto> Authenticate(string email, string password)
        {
            var user = await _authenticationRepository.Authenticate(email, password);
            if (user == null)
                return null;

            var userResponseDto = new UserResponseDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role,
                Balance = user.Balance,
                Token = CreateJwt(user)
            };

            return userResponseDto;
        }

        public async Task RegisterUser(UserDto userDto)
        {
            await _authenticationRepository.RegisterUser(userDto);
        }

        private string CreateJwt(UserEF user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("your_secret_key_here_and_this_must_be_long_so_that_it_can_create_hash"); // Replace with your secret key
            var identity = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("balance", user.Balance.ToString()),
            new Claim("userid", user.Id.ToString())
            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
