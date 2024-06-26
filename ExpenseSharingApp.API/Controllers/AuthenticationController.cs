using ExpenseSharingApp.BLL.IServices;
using ExpenseSharingApp.BLL.Models;
using ExpenseSharingApp.Common.UserDTOs;
using ExpenseSharingApp.DAL.EF;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExpenseSharingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServices _authenticationService;

        public AuthenticationController(IAuthenticationServices authenticationService)
        {
            _authenticationService = authenticationService;
        }
        //[HttpPost("authenticate")]
        //public async Task<IActionResult> Authenticate([FromBody] UserEF userObj)
        //{
        //    if (userObj == null)
        //    {
        //        return BadRequest();
        //    }

        //    var user = await _authenticationService.Authenticate(userObj.Email, userObj.PasswordHash);

        //    if (user == null)
        //    {
        //        return NotFound(new { message = "User Not Found" });
        //    }

        //    user.Token = CreateJwt(user);

        //    return Ok(new
        //    {
        //        token = user.Token,
        //        message = "Login successful!"
        //    });
        //}

        //[HttpPost("register")]
        //public async Task<IActionResult> Register([FromBody] UserEF userObj)
        //{
        //    if (userObj == null)
        //    {
        //        return BadRequest();
        //    }

        //    await _authenticationService.RegisterUser(userObj);

        //    return Ok(new
        //    {
        //        Message = "User registered successfully."
        //    });
        //}
        //private string CreateJwt(UserEF user)
        //{
        //    var jwtTokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes("your_secret_key_here_ NEWBCIWVWQVCSDUCWFWQIUFCHaecscsdvasvhasd vhsadvbsksjb"); // Replace with your secret key
        //    var identity = new ClaimsIdentity(new Claim[]
        //    {
        //    new Claim(ClaimTypes.Name, user.UserName),
        //    new Claim(ClaimTypes.Role, user.Role),
        //    new Claim("balance", user.Balance.ToString()),
        //    new Claim("userid", user.Id.ToString())
        //    });
        //    var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = identity,
        //        Expires = DateTime.Now.AddDays(1),
        //        SigningCredentials = credentials
        //    };
        //    var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        //    return jwtTokenHandler.WriteToken(token);
        //}

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }

            var userResponseDto = await _authenticationService.Authenticate(userDto.Email, userDto.Password);

            if (userResponseDto == null)
            {
                return NotFound(new { message = "User Not Found" });
            }

            return Ok(new
            {
                token = userResponseDto.Token,
                message = "Login successful!",
                user = userResponseDto
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest();
            }

            await _authenticationService.RegisterUser(userDto);

            return Ok(new
            {
                Message = "User registered successfully."
            });
        }
    }
}
