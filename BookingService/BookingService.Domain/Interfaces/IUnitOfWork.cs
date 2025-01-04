namespace BookingService.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IApartmentRepository ApartmentRepository { get; }
        IWalletRepository WalletRepository { get; }
        IBookingRepository BookingRepository { get; }
        IUserManager UserManager { get; }
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task<int> SaveChangesAsync();
    }
}
