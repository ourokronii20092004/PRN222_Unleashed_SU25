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
        CreateMap<Notification, NotificationDetailDTO>();


        CreateMap<NotificationDetailDTO, Notification>();
        CreateMap<NotificationCreateDTO, Notification>();
        }
    }
}
