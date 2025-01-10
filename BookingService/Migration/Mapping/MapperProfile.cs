using AutoMapper;
using BookingService.Domain.Entities;
using Migration.DataModels;

namespace Migration.Mapping
{
    internal class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserDataModel, User>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<ApartmentDataModel, Apartment>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}