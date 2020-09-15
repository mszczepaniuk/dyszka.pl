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

            CreateMap<AuditLog, AuditLogVm>()
                .ForMember(dest => dest.AuthorUsername, opt => opt.MapFrom(src => src.CreatedBy.UserName));

            CreateMap<OfferBm, Offer>();
            CreateMap<Offer, OfferVm>()
                .ForMember(dest => dest.AuthorUserName, opt => opt.MapFrom(src => src.CreatedBy.UserName));
        }
    }
}