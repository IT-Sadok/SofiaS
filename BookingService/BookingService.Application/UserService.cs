using BookingService.Application.DTOs;
using BookingService.Domain;
using Microsoft.AspNetCore.Identity;

namespace BookingService.Application
{
    public class UserService
    {
        private ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return await _tokenService.GenerateToken(user);
            }
            return null;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                 await _userManager.AddToRoleAsync(user, registerDto.Role);
            }
            return result;
        }

    }
}