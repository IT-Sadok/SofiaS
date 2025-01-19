using BookingService.Application.DTOs;
using BookingService.Domain.Entities;

namespace BookingService.Application.Abstract
{
    public interface IApartmentService
    {
        Task<Result<int>> CreateApartmentAsync(string hostId, ApartmentCreateDto apartmentDto);
        Task<Result> UpsertCustomDataAsync(int apartmentId, string hostId, List<ApartmentUpsertDto> customData);
    }
}