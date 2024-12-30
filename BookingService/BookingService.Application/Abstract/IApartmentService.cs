using BookingService.Application.DTOs;
using BookingService.Domain.Models;

namespace BookingService.Application.Abstract
{
    public interface IApartmentService
    {
        Task<Result<int>> CreateApartmentAsync(ApartmentCreateDto apartmentDto);
    }
}