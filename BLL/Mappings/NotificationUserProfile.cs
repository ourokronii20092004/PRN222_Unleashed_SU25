
using AutoMapper;
using DAL.DTOs.NotificationDTOs;
using DAL.Models;

namespace BLL.Mappings
{
    public class NotificationUserProfile : Profile
    {
        public NotificationUserProfile() { 
            CreateMap<NotificationUser, NotificationUserDetailDTO>()
                .ForMember(notiuDTO => notiuDTO.NotificationTitle, opt => opt.MapFrom(not => not.Notification.NotificationTitle))
                .ForMember(notiuDTO => notiuDTO.NotificationCreatedAt, opt => opt.MapFrom(not => not.Notification.NotificationCreatedAt));
            
            CreateMap<NotificationUserDetailDTO, NotificationUser>();
        }
    }
}
