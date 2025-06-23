using AutoMapper;
using DAL.Models;
using DAL.DTOs;

namespace BLL.Mappings
{
    public class StockProfile : Profile
    {
        public StockProfile()
        {
            CreateMap<Stock, StockDTO>(); // For listing, details if simple
            CreateMap<StockDTO, Stock>(); // If needed for mapping back

            CreateMap<StockCreateDTO, Stock>(); // For creating new stocks
            CreateMap<StockUpdateDTO, Stock>(); // For applying updates
            CreateMap<Stock, StockUpdateDTO>(); // For populating edit form
            CreateMap<StockDTO, StockUpdateDTO>();
        }
    }
}