using Microsoft.AspNetCore.Identity;

namespace BookingService.Domain.Interfaces
{
    public interface IRoleManager
    {
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityRole> FindByName(string roleName);
    }
}