using BookingService.Application.DTOs;
using BookingService.Domain;
using Microsoft.AspNetCore.Identity;

namespace BookingService.Application.Abstract
{
    public interface ICustomUserManager
    {
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<User> FindByNameAsync(string userName);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}
