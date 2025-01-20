using BookingService.Application.DTOs;
using BookingService.Domain.DTOs;
using BookingService.Domain.Entities;

namespace BookingService.Application.Abstract
{
    public interface IStatisticService
    {
        Task<Result<IEnumerable<ApartmentPriceQuantilesQueryResult>>> GetApartmentPriceQuantiles();
        Task<Result<IEnumerable<BookingDurationQueryResult>>> GetAverageBookingDurationPerApartment();
        Task<Result<IEnumerable<HostsProfitQueryResult>>> GetHostsProfit();
        Task<Result<IEnumerable<RepeatedBookingQueryResult>>> GetRepeatedBookingPerApartmentClient();
        Task<Result<IEnumerable<BookedApartmentQueryResult>>> GetTopMostBookedApartment(int n);
    }
}
