using AutoMapper;
using DAL.DTOs.VariationDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    public class VariationProfile : Profile
    {
        public VariationProfile()
        {

            // deatiled bc it fucked up
            // no change pls
            CreateMap<Variation, VariationDetailDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Product.Brand.BrandName))
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Size.SizeName))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.ColorName))
                .ForMember(dest => dest.VariationImageUrl, opt => opt.MapFrom(src => src.VariationImage))
                .ForMember(dest => dest.ColorHexCode, opt => opt.MapFrom(src => src.Color.ColorHexCode))
                .ForMember(dest => dest.VariationPrice, opt => opt.MapFrom(src => src.VariationPrice));
        }
    }
}
