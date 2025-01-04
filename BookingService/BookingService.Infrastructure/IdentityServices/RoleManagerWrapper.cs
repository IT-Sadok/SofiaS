using BookingService.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BookingService.Infrastructure.IdentityServices
{
    public class RoleManagerWrapper : IRoleManager
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagerWrapper(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}
