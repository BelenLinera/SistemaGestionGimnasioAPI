using AutoMapper;
using SistemaGestionGimnasioApi.Data.Entities;

namespace SistemaGestionGimnasioApi.Data.Models
{
    public class ActivityProfile: Profile
    {
        public ActivityProfile() 
        {
            CreateMap<ActivityDto, Activity>()
                .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(src => src.ActivityName))
            
                .ForMember(dest => dest.ActivityDescription, opt => opt.MapFrom(src => src.ActivityDescription));
        }
    }
    
    
}
