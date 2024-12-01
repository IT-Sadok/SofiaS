using AutoMapper;
using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
using BookingService.Domain;
using Microsoft.AspNetCore.Identity;

namespace BookingService.Application
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly ICustomUserManager _userManager;
        private readonly IMapper _mapper;

        public UserService(ITokenService tokenService, ICustomUserManager userManager, IMapper mapper)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _mapper = mapper;
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
            var user = _mapper.Map<User>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            return result;
        }

    }
}