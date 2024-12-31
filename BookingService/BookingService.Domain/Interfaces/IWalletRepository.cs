using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet?> FindWalletByUserIdAsync(string userId);
        Task UpdateAsync(Wallet wallet);
    }
}