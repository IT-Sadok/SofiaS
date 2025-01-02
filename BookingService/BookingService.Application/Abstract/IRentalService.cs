using BookingService.Application.DTOs;
using BookingService.Domain.Entities;

namespace BookingService.Application.Abstract
{
    public interface IRentalService
    {
        Task<Result> CreateBooking(string tenantId, BookingCreateDto bookingCreateDto);
    }
}
