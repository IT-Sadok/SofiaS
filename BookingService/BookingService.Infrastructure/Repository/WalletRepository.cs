using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Repository
{
    public class WalletRepository : IWalletRepository
    {
        private readonly AppDbContext _context;

        public WalletRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet?> FindWalletByUserIdAsync(string userId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
            //TODO: think about what to use Find, FirstOrDefault,...
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            if (wallet != null)
            {
                _context.Wallets.Update(wallet);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsyncByUserId(string userId)
        {
            var wallet = await FindWalletByUserIdAsync(userId);
            if (wallet != null)
            {
                _context.Wallets.Update(wallet);
                await _context.SaveChangesAsync();
            }
        }
    }
}