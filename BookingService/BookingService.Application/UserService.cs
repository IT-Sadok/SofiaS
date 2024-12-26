using AutoMapper;
using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
using BookingService.Domain;
using BookingService.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace BookingService.Application
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly ICustomUserManager _userManager;
        private readonly ICustomRoleManager _roleManager;
        private readonly IMapper _mapper;

        public UserService(ITokenService tokenService, ICustomUserManager userManager, ICustomRoleManager roleManager, IMapper mapper)
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
                return await _tokenService.GenerateToken(user);
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
                return IdentityResult.Failed(new IdentityError { Description = "Role not found"});
            }

            await _userManager.AddToRoleAsync(user, registerDto.Role);
            return IdentityResult.Success;
        }

    }
}