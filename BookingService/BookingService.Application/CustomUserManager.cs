using BookingService.Application.Abstract;
using BookingService.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace BookingService.Application
{
    public class CustomUserManager : ICustomUserManager
    {
        private readonly UserManager<User> _userManager;

        public CustomUserManager(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> AddToRoleAsync(User user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }
    }
}