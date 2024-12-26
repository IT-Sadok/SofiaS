using BookingService.Domain.Models;

namespace BookingService.Domain
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }
}
