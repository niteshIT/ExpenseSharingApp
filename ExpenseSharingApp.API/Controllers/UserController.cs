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
        private readonly ExpenseSharingContext _context;

        public UserController(IUserService userService, ExpenseSharingContext context)
        {
            _userService = userService;
            _context = context;
        }

        

        [HttpGet]
        //[Authorize(Roles = "admin, user")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var allUsers = await _userService.GetAllUsers();
            return Ok(allUsers);
        }

        [HttpGet("{userId}")]
        //[Authorize(Roles = "admin, user")]
        public async Task<ActionResult<User>> GetUser(int userId)
        {
            var user = await _userService.GetUser(userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            return Ok(user);
        }
        [HttpGet("group/{groupId}")]
        //[Authorize(Roles = "admin, user")]
        public async Task<ActionResult<IEnumerable<UserEF>>> GetUsersByGroupId(int groupId)
        {
            var users = await _context.UserGroups
                                      .Where(ug => ug.GroupId == groupId)
                                      .Include(ug => ug.User)
                                      .Select(ug => ug.User)
                                      .ToListAsync();

            if (users == null || users.Count == 0)
            {
                return NotFound();
            }

            return Ok(users);
        }
    }
}
