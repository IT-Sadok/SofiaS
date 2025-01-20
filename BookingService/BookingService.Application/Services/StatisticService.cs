using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
using BookingService.Domain.DTOs;
using BookingService.Domain.Entities;
using BookingService.Domain.Interfaces;

namespace BookingService.Application.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IApartmentRepository _apartmentRepository;

        public StatisticService(IBookingRepository bookingRepository, IApartmentRepository apartmentRepository)
        {
            _bookingRepository = bookingRepository;
            _apartmentRepository = apartmentRepository;
        }

        public async Task<Result<IEnumerable<HostsProfitQueryResult>>> GetHostsProfit()
        {
            var hostsWithProfit = await _bookingRepository.GetHostsProfit();
            return Result<IEnumerable<HostsProfitQueryResult>>.Success(hostsWithProfit);
        }

        public async Task<Result<IEnumerable<BookedApartmentQueryResult>>> GetTopMostBookedApartment(int n)
        {
            var topApartments = await _bookingRepository.GetTopMostBookedApartment(n);
            return Result<IEnumerable<BookedApartmentQueryResult>>.Success(topApartments);
        }

        public async Task<Result<IEnumerable<ApartmentPriceQuantilesQueryResult>>> GetApartmentPriceQuantiles()
        {
            var priceQuantiles = await _apartmentRepository.GetPriceQuantiles();
            return Result<IEnumerable<ApartmentPriceQuantilesQueryResult>>.Success(priceQuantiles);
        }

        public async Task<Result<IEnumerable<RepeatedBookingQueryResult>>> GetRepeatedBookingPerApartmentClient()
        {
            var repeatedBookings = await _bookingRepository.GetRepeatedBookingPerApartmentClient();
            return Result<IEnumerable<RepeatedBookingQueryResult>>.Success(repeatedBookings);
        }

        public async Task<Result<IEnumerable<BookingDurationQueryResult>>> GetAverageBookingDurationPerApartment()
        {
            var avgBookingDuration = await _bookingRepository.GetAverageBookingDurationPerApartment();
            return Result<IEnumerable<BookingDurationQueryResult>>.Success(avgBookingDuration);
        }
    }
}