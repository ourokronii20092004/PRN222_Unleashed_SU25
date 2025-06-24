using AutoMapper;
using DAL.Models;
using DAL.DTOs;

namespace BLL.Mappings
{
    public class StockProfile : Profile
    {
        public StockProfile()
        {
            // Entity to DTO
            CreateMap<Stock, StockDTO>();
            CreateMap<Stock, StockUpdateDTO>();
            
            // DTO to Entity
            CreateMap<StockCreateDTO, Stock>();
            CreateMap<StockUpdateDTO, Stock>();
            CreateMap<StockDTO, Stock>();

            // DTO to DTO
            CreateMap<StockDTO, StockUpdateDTO>();

        }
    }
}