using AutoMapper;
using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
using BookingService.Domain.Interfaces;
using BookingService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BookingService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;
        private readonly IMapper _mapper;

        public AuthService(ITokenService tokenService, IUserManager userManager, IRoleManager roleManager, IMapper mapper)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                return await _tokenService.GenerateToken(user, roles);
            }
            return null;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return result;
            }

            var roleExists = await _roleManager.RoleExistsAsync(registerDto.Role);

            if (!roleExists)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role was not found." });
            }

            return await _userManager.AddToRoleAsync(user, registerDto.Role);
        }
    }
}