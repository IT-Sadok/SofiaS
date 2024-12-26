using BookingService.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace BookingService.Application.Abstract
{
    public interface ICustomUserManager
    {
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<User> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IdentityResult> AddToRoleAsync(User user, string role);
    }
}