using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IApartmentRepository
    {
        public Task<int> CreateAsync(Apartment apartment);
        Task<Apartment?> FindByIdAsync(int apartmentId);
        Task UpdateAsync(Apartment apartment);
        bool IsAvailable(int apartmentId, DateTime startDate, DateTime endDate);
    }
}