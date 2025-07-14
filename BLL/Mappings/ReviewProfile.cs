using AutoMapper;
using DAL.DTOs.ProductDTOs;
using DAL.DTOs.ReviewDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDTO>().ReverseMap();
            CreateMap<Review, ReviewDetailDTO>()              
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            CreateMap<ReviewCreateDTO, Review>();
        }
    }
}
