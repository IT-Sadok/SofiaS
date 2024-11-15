using BookingService.Application;
using BookingService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Register([FromBody] RegisterDto registerdto)
        {
            var result = await _userService.RegisterAsync(registerdto);
            if (result.Succeeded)
                return Ok("Registration successful");

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var token = await _userService.LoginAsync(loginDto);
            if (string.IsNullOrEmpty(token))
                return Unauthorized();

            return Ok(new { Token = token });
        }
    }
}
