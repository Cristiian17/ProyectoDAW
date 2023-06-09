using Api.Context;
using Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public UsersController(DataBaseContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            var user = HttpContext.User;

            var userId = user.FindFirst("userId")?.Value;
            var email = user.FindFirst(ClaimTypes.Email)?.Value;

            var users = _context.Users.ToList();

            return users;
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (user == null)
            {
                return BadRequest("Error 404");
            }

            var existUser = _context.Users.FirstOrDefault(x => x.Email.Equals(user.Email));

            if (existUser != null)
            {
                return BadRequest("El usuario ya está registrado");
            }

            User newUser = new User { Email = user.Email, Password = user.Password, Name = user.Name };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return Ok(true);
        }

        [Authorize]
        [HttpGet("{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email.Equals(email));

            if (user == null)
            {
                return BadRequest(false);
            }

            return Ok(true);
        }
    }
}
