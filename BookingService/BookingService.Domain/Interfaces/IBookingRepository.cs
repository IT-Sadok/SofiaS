using BookingService.Application.DTOs;
using BookingService.Domain.DTOs;
using BookingService.Domain.Entities;

namespace BookingService.Domain.Interfaces
{
    public interface IBookingRepository
    {
        Task<int> CreateAsync(Booking booking);
        Task<IEnumerable<BookingDurationQueryResult>> GetAverageBookingDurationPerApartment();
        Task<IEnumerable<HostsProfitQueryResult>> GetHostsProfit();
        Task<IEnumerable<RepeatedBookingQueryResult>> GetRepeatedBookingPerApartmentClient();
        Task<IEnumerable<BookedApartmentQueryResult>> GetTopMostBookedApartment(int n);
    }
}
