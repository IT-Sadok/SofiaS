using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task<int> CreateAsync(Booking booking);
    }
}
