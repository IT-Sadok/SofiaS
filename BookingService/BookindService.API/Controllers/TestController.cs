using BookingService.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("admin")]
        [Authorize(Roles = Roles.Admin)]
        public IActionResult GetAdminData()
        {
            return Ok("This is a test endpoint for Admin role");
        }

        [HttpGet("host")]
        [Authorize(Roles = Roles.Host)]
        public IActionResult GetHostData()
        {
            return Ok("This is a test endpoint for Host role");
        }

        [HttpGet("user")]
        [Authorize(Roles = Roles.User)]
        public IActionResult GetUserData()
        {
            return Ok("This is a test endpoint for User role");
        }

    }
}
