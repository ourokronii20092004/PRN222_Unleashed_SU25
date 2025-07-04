using AutoMapper;
using DAL.DTOs.BrandDTOs;
using DAL.Models;

namespace BLL.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            // Entity to DTO
            CreateMap<Brand, BrandDTO>();
            CreateMap<Brand, SearchBrandDTO>();

            // DTO to Entity
            CreateMap<BrandCreateDTO, Brand>();
            CreateMap<BrandUpdateDTO, Brand>();
            CreateMap<BrandDTO, Brand>();

            //DTO to DTO
            CreateMap<BrandDTO, BrandUpdateDTO>();
        }
    }
}
