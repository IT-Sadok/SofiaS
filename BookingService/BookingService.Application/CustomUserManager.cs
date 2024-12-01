using BookingService.Application.Abstract;
using BookingService.Domain;
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

        public async Task<User> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}
