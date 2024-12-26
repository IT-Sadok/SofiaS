using BookingService.Application.Abstract;
using Microsoft.AspNetCore.Identity;

namespace BookingService.Application
{
    public class CustomRoleManager : ICustomRoleManager
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomRoleManager(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}
