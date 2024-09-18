using AutoMapper;
using NestWebApp.Models;
using NestWebApp.Models.ViewModels;

namespace NestWebApp.Mapping
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<AppUser, UserListVM>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
            CreateMap<AppUser, GetUserVM>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore());
        }
    }
}
