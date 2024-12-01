using BookingService.Application.DTOs;
using Microsoft.AspNetCore.Identity;

namespace BookingService.Application.Abstract
{
    public interface IUserService
    {
        Task<string> LoginAsync(LoginDto loginDto);
        Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
    }
}
