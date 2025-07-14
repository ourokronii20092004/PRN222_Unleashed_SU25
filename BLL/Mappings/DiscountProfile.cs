using AutoMapper;
using DAL.DTOs.DiscountDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    public class DiscountProfile : Profile
    {
        public DiscountProfile()
        {
            CreateMap<Discount, DiscountDTO>()
                .ForMember(dest => dest.DiscountStatusName, opt => opt.MapFrom(src => src.DiscountStatus.DiscountStatusName))
                .ForMember(dest => dest.DiscountTypeName, opt => opt.MapFrom(src => src.DiscountType.DiscountTypeName));

            CreateMap<DiscountCreateDTO, Discount>();
            CreateMap<DiscountUpdateDTO, Discount>();

            CreateMap<DiscountDTO, DiscountUpdateDTO>();
            CreateMap<Discount, DiscountUpdateDTO>();
        }
    }
}
