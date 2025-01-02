using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IApartmentRepository
    {
        public Task<int> AddApartmentAsync(Apartment apartment);
        Task<Apartment?> FindApartmentByIdAsync(int apartmentId);
        Task UpdateAsync(Apartment apartment);
    }
}