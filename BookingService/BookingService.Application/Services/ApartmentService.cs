using AutoMapper;
using BookingService.Application.Abstract;
using BookingService.Application.DTOs;
using BookingService.Domain.Interfaces;
using BookingService.Domain.Entities;
using System.Text.Json;

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

        public async Task<Result<int>> CreateApartmentAsync(string hostId, ApartmentCreateDto apartmentDto)
        {
            var apartment = _mapper.Map<Apartment>(apartmentDto);
            apartment.HostId = hostId;
            int id = await _apartmentRepository.CreateAsync(apartment);
            return Result<int>.Success(id);
        }

        public async Task<Result> UpsertCustomDataAsync(int apartmentId, string hostId, List<ApartmentUpsertDto> customData)
        {
            var dictionary = customData.ToDictionary(cd => cd.Key, cd => cd.Value);
            var customDataJson = JsonSerializer.Serialize(dictionary);

            await _apartmentRepository.UpsertCustomData(apartmentId, hostId, customDataJson);

            return Result.Success();
        }
    }
}