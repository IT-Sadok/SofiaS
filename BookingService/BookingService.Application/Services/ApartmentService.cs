using AutoMapper;
using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
using BookingService.Domain.Interfaces;
using BookingService.Domain.Models;

namespace BookingService.Application.Services
{
    public class ApartmentService : IApartmentService
    {
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IMapper _mapper;

        public ApartmentService(IApartmentRepository apartmentRepository, IMapper mapper)
        {
            _apartmentRepository = apartmentRepository;
            _mapper = mapper;
        }

        public async Task<Result<int>> CreateApartmentAsync(ApartmentCreateDto apartmentDto)
        {
            var apartment = _mapper.Map<Apartment>(apartmentDto);
            int id = await _apartmentRepository.AddApartmentAsync(apartment);
            return Result<int>.Success(id);
        }
    }
}