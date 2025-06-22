using AutoMapper;
using DAL.DTO;
using DAL.Models;

namespace BLL.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            // Entity to DTO
            CreateMap<Brand, BrandDTO>();
            CreateMap<Brand, SearchBrandDTO>(); // Assuming BrandName and BrandDescription are directly on Brand

            // DTO to Entity
            CreateMap<BrandCreateDTO, Brand>(); // Ignores BrandId, BrandCreatedAt, BrandUpdatedAt as they are set manually or by DB
            CreateMap<BrandUpdateDTO, Brand>(); // Ignores BrandId, BrandCreatedAt, BrandUpdatedAt

            CreateMap<BrandDTO, Brand>();
        }
    }
}
