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


        [HttpPost("{userId}/wallet/top-up")]
        [Authorize(Roles = Roles.User)]
        public async Task<ActionResult> TopUpBalance(string userId, [FromBody] WalletTopUpDto walletTopUpDto)
        {
            var result = await _userService.TopUpBalance(userId, walletTopUpDto);
            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok("Balance successfully was topped up");
        }

    }
}
