using BookingService.Application.DTOs;
using BookingService.Domain.Entities;

namespace BookingService.Application.Abstract
{
    public interface IRentalService
    {
        Task<Result<int>> CreateBooking(string clientId, BookingCreateDto bookingCreateDto);
    }
}
