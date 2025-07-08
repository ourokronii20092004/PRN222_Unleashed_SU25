using AutoMapper;
using DAL.DTOs.NotificationDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile() { 
            CreateMap<Notification, NotificationDetailDTO>()
                .ForMember(notiDTO => notiDTO.IsNotificationDraft, opt => opt.MapFrom(noti => noti.IsNotificationDraft))
                .ForMember(notiDTO => notiDTO.UserIdSenderNavigation, opt => opt.MapFrom(noti => noti.UserIdSenderNavigation)); ;



            CreateMap<NotificationDetailDTO, Notification>()
                .ForMember(noti => noti.NotificationCreatedAt, opt => opt.Ignore())
                .ForMember(noti => noti.UserIdSenderNavigation, opt => opt.Ignore())
                .ForMember(noti => noti.UserIdSender, opt => opt.Ignore())
                .ForMember(noti => noti.IsNotificationDraft, opt => opt.MapFrom(notiDTO => notiDTO.IsNotificationDraft)); ;

            CreateMap<NotificationCreateDTO, Notification>()
                .ForMember(noti => noti.IsNotificationDraft, opt => opt.MapFrom(notiDTO => notiDTO.IsNotificationDraft));
        }
    }
}
