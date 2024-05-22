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
                .ForMember(dest => dest.TimeClass, opt => opt.MapFrom(src => src.TimeClass))
                .ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.Days))
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity));


        }
    }
}
