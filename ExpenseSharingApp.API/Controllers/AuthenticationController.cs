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
