using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IWalletRepository
    {
        Task<Wallet?> FindByUserIdAsync(string userId);
        Task UpdateAsync(Wallet wallet);
        Task UpdateAsync(string userId);
    }
}