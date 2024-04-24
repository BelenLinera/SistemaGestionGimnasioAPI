using SistemaGestionGimnasioApi.Data.Entities;
using AutoMapper;
using SistemaGestionGimnasioApi.Data.Models;

namespace SistemaGestionGimnasioApi.Data.Profiles
{
    public class AdminProfile:Profile
    {
        public AdminProfile()
        {
            CreateMap<UserDto, Admin>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<EditUserDto, Admin>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));
        }
    }
}
