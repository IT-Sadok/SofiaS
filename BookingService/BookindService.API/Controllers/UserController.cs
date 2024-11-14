using BookindService.API.ViewModels;
using BookingService.Application;
using BookingService.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace BookindService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterModel registerModel)
        {

        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            var token = _userService.Authenticate(new User { UserName = loginModel.Username });

            if (string.IsNullOrEmpty(token))
                return Unauthorized();

            return Ok(new { Token = token });
        }
    }
}
