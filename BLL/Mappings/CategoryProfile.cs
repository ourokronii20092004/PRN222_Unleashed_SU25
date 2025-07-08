using AutoMapper;
using DAL.DTOs.BrandDTOs;
using DAL.DTOs.CategoryDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            // Entity to DTO
            CreateMap<Category, CategoryDTO>();
            CreateMap<Category, CategoryDTO>();

            // DTO to Entity
            CreateMap<CategoryCreateDTO, Category>();
            CreateMap<CategoryUpdateDTO, Category>();
            CreateMap<CategoryDTO, Category>();

            //DTO to DTO
            CreateMap<CategoryDTO, CategoryUpdateDTO>();
        }
    }
}
