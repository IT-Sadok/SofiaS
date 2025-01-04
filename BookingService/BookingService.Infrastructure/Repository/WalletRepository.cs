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

        public async Task<Wallet?> FindByUserIdAsync(string userId)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == userId);
        }

        public async Task<List<Wallet>> FindByUserIdsAsync(params string[] userIds)
        {
            return await _context.Wallets
                .Where(w => userIds.Contains(w.UserId))
                .ToListAsync();
        }

        public async Task UpdateAsync(Wallet wallet)
        {
            if (wallet != null)
            {
                _context.Wallets.Update(wallet);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(string userId)
        {
            var wallet = await FindByUserIdAsync(userId);
            if (wallet != null)
            {
                _context.Wallets.Update(wallet);
                await _context.SaveChangesAsync();
            }
        }
    }
}