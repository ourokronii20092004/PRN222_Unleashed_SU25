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
            // Entity -> DTO
            CreateMap<Discount, DiscountDTO>()
                .ForMember(dest => dest.DiscountStatusName, opt => opt.MapFrom(src => src.DiscountStatus.DiscountStatusName))
                .ForMember(dest => dest.DiscountTypeName, opt => opt.MapFrom(src => src.DiscountType.DiscountTypeName));

            // DTO -> Entity
            CreateMap<DiscountCreateDTO, Discount>();
            CreateMap<DiscountUpdateDTO, Discount>();

            // DTO -> DTO (để lấy dữ liệu cho form Edit)
            CreateMap<DiscountDTO, DiscountUpdateDTO>();
            CreateMap<Discount, DiscountUpdateDTO>();
        }
    }
}
