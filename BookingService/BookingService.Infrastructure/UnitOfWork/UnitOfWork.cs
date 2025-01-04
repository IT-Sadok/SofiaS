using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookingService.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;
        public IApartmentRepository ApartmentRepository { get; private set; }
        public IWalletRepository WalletRepository { get; private set; }
        public IBookingRepository BookingRepository { get; private set; }
        public IUserManager UserManager { get; private set; }

        public UnitOfWork(AppDbContext context,
                            IApartmentRepository apartmentRepository,
                            IWalletRepository walletRepository,
                            IBookingRepository bookingRepository,
                            IUserManager userManager)
        {
            _context = context;
            ApartmentRepository = apartmentRepository;
            WalletRepository = walletRepository;
            BookingRepository = bookingRepository;
            UserManager = userManager;
        }


        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
