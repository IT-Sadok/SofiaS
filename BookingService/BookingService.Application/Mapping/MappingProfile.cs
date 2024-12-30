using AutoMapper;
using BookingService.Application.DTOs;
using BookingService.Domain.Models;

namespace BookingService.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));

            CreateMap<ApartmentCreateDto, Apartment>();
        }
    }
}