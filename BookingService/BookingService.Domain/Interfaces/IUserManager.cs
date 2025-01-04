using BookingService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BookingService.Domain.Interfaces
{
    public interface IUserManager
    {
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<User?> FindByEmailAsync(string email);
        Task<User?> FindByIdAsync(string userId);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
        Task<IList<string>> GetRolesAsync(User user);
        Task<List<User>> FindByIdsAsync(params string[] userIds);
    }
}