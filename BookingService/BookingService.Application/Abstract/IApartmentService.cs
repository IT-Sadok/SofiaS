using BookingService.Application.DTOs;
using BookingService.Domain.Entities;

namespace BookingService.Application.Abstract
{
    public interface IApartmentService
    {
        Task<Result<int>> CreateApartmentAsync(ApartmentCreateDto apartmentDto);
    }
}