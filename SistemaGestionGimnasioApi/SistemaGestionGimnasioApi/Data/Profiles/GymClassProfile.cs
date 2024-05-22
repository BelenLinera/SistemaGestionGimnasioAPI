using AutoMapper;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Data.Profiles
{
    public class GymClassProfile:Profile
    {
        public GymClassProfile() 
        { 
            CreateMap<GymClassDto, GymClass>()
                .ForMember(dest => dest.IdTrainerActivity, opt => opt.MapFrom(src => src.IdTrainerActivity))
                .ForMember(dest => dest.DateTimeClass, opt => opt.MapFrom(src => src.DateTimeClass))
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity));

            CreateMap<EditGymClassDto, GymClass>()
                .ForMember(dest => dest.IdTrainerActivity, opt => opt.MapFrom(src => src.IdTrainerActivity))
                .ForMember(dest => dest.DateTimeClass, opt => opt.MapFrom(src => src.DateTimeClass))
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity));

        }
    }
}
