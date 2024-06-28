using AutoMapper;
using SistemaGestionGimnasioApi.Data.Entities;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Data.Profiles
{
    public class ReserveProfile: Profile
    {
        public ReserveProfile() 
        {
            CreateMap<ReserveDto, Reserve>()
                .ForMember(dest => dest.IdGymClass, opt => opt.MapFrom(src => src.IdGymClass))
                .ForMember(dest => dest.DateClass, opt => opt.MapFrom(src => src.DateClass));
        }
    }
}
