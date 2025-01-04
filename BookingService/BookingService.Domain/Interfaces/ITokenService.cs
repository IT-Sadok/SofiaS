using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user, IEnumerable<string> roles);
    }
}
