using AutoMapper;
using DAL.DTOs.ProviderDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mappings
{
    public class ProviderProfile : Profile
    {
        public ProviderProfile()
        {
            // Mapping cho danh sách (đã có)
            CreateMap<Provider, ProviderBareboneDTO>();

            // Mapping cho trang chi tiết
            CreateMap<Provider, ProviderDetailsDTO>();

            // Mapping để tạo mới Provider
            CreateMap<ProviderCreateDTO, Provider>();

            // Mapping từ Provider sang DTO để chỉnh sửa
            CreateMap<Provider, ProviderEditDTO>();

            // Mapping từ DTO đã chỉnh sửa về lại Provider
            CreateMap<ProviderEditDTO, Provider>();
        }


    }
}
