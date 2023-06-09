using Api.Authentication;
using Api.Context;
using Api.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(DataBaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost]
        public IActionResult Login([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Email o contraseña incorrectos");
            }

            var userDB = _context.Users.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);

            if (userDB == null)
            {
                return BadRequest("Email o contraseña incorrectos");
            }

            var token = JwtAuthenticationExtensions.GetUserToken(userDB, _configuration);

            var userInfo = new UserInfo()
            {
                Email = userDB.Email,
                Id = userDB.Id,
                UserName = userDB.Name,
                Token = token
            };

            return Ok(userInfo);
        }
        [HttpPost]
        [Route("RegenerateToken")]
        public IActionResult RegenerateToken(UserInfo userInfo)
        {
            var userDB = _context.Users.FirstOrDefault(x => x.Email == userInfo.Email && x.Id == userInfo.Id);
            var token = JwtAuthenticationExtensions.GetUserToken(userDB, _configuration);
            var newUserInfo = new UserInfo()
            {
                Email = userDB.Email,
                Id = userDB.Id,
                UserName = userDB.Name,
                Token = token
            };
            return Ok(newUserInfo);
        }
    }
}
