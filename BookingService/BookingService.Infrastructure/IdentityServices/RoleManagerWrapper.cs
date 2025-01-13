using BookingService.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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
        
        public async Task<IdentityRole?> FindByName(string roleName)
        {
            return await _roleManager.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        }
    }
}