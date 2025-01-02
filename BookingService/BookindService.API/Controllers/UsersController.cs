using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
using BookingService.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("/wallet/top-up")]
        [Authorize(Roles = Roles.User)]
        public async Task<ActionResult> TopUpBalance([FromBody] WalletTopUpDto walletTopUpDto)
        {
            var userId = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (userId == null)
            {
                return BadRequest();
            }

            var result = await _userService.TopUpBalance(userId, walletTopUpDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok("Balance successfully was topped up");
        }

    }
}
