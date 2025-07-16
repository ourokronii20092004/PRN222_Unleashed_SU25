using AutoMapper;
using DAL.DTOs.OderDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDTO>()
            .ForMember(dest => dest.OrderStatusId, opt => opt.MapFrom(src => src.OrderStatusId)); // QUAN TRỌNG
            
            CreateMap<OrderDTO, Order>();
            CreateMap<OrderVariationSingle, OrderDetailDTO>();
            CreateMap<OrderDetailDTO, OrderVariationSingle>();

        }
        
    }
}
