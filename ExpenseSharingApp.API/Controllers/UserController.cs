using ExpenseSharingApp.BLL.IServices;
using ExpenseSharingApp.BLL.Models;
using ExpenseSharingApp.DAL.Data;
using ExpenseSharingApp.DAL.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ExpenseSharingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
       

        public UserController(IUserService userService)
        {
            _userService = userService;
           
        }

        

        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult<IEnumerable<UserEF>>> GetAllUsers()
        {
            var allUsers = await _userService.GetAllUsers();
            return Ok(allUsers);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult<UserEF>> GetUser(int userId)
        {
            var user = await _userService.GetUser(userId);

            if (user == null)
            {
                return NotFound( "User not found." );
            }

            return Ok(user);
        }
        

        [HttpGet("group/{groupId}")]
        [Authorize(Roles = "admin, user")]
        public async Task<ActionResult<IEnumerable<UserEF>>> GetUsersByGroupId(int groupId)
        {
            var users = await _userService.GetUsersByGroupIdAsync(groupId);

            if (users == null || !users.Any())
            {
                return NotFound();
            }

            return Ok(users);
        }
    }
}
