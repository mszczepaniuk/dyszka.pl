using ApplicationCore.BindingModels;
using ApplicationCore.Models;
using ApplicationCore.ViewModels;
using AutoMapper;

namespace Web.Mappings
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            CreateMap<ApplicationUserBm, ApplicationUser>();
            CreateMap<ApplicationUser, ApplicationUserVm>();
        }
    }
}