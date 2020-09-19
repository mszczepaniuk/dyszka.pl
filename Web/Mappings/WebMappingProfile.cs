using ApplicationCore.BindingModels;
using ApplicationCore.Models;
using ApplicationCore.ViewModels;
using AutoMapper;
using Web.Mappings.Resolvers;

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

            CreateMap<CommentBm, Comment>()
                .ForMember(dest => dest.Offer, opt => opt.MapFrom<CommentResolver>());
            CreateMap<Comment, CommentVm>()
                .ForMember(dest => dest.AuthorUserName, opt => opt.MapFrom(src => src.CreatedBy.UserName));

            CreateMap<MessageBm, Message>()
                .ForMember(dest => dest.Receiver, opt => opt.MapFrom<MessageResolver>());
            CreateMap<Message, MessageVm>()
                .ForMember(dest => dest.AuthorUserName, opt => opt.MapFrom(src => src.CreatedBy.UserName))
                .ForMember(dest => dest.ReceiverUserName, opt => opt.MapFrom(src => src.Receiver.UserName));

            CreateMap<Order, OrderVm>()
                .ForMember(dest => dest.OfferId, opt => opt.MapFrom(src => src.Offer.Id))
                .ForMember(dest => dest.OfferAuthorUserName, opt => opt.MapFrom(src => src.Offer.CreatedBy.UserName))
                .ForMember(dest => dest.OfferTitle, opt => opt.MapFrom(src => src.Offer.Title))
                .ForMember(dest => dest.AuthorUserName, opt => opt.MapFrom(src => src.CreatedBy.UserName));
        }
    }
}