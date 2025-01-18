using BookingService.Domain.DTOs;
using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IApartmentRepository
    {
        Task<int> CreateAsync(Apartment apartment);
        Task<Apartment?> FindByIdAsync(int apartmentId);
        Task UpdateAsync(Apartment apartment);
        bool IsAvailable(int apartmentId, DateTime startDate, DateTime endDate);
        Task<IEnumerable<ApartmentPriceQuantilesQueryResult>> GetPriceQuantiles();
    }
}