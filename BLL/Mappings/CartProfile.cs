using DAL.DTOs.CartDTOs;
using DAL.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    public class CartProfile : Profile
    {
        public CartProfile() 
        {

            CreateMap<Cart, CartDTO>()
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.CartQuantity))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Variation.Product.ProductName))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Variation.VariationImage))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Variation.Color.ColorName))
                .ForMember(dest => dest.SizeName, opt => opt.MapFrom(src => src.Variation.Size.SizeName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Variation.VariationPrice));

            CreateMap<AddToCartDTO, Cart>()
                .ForMember(dest => dest.CartQuantity, opt => opt.MapFrom(src => src.Quantity));
        }
        
    }
}
