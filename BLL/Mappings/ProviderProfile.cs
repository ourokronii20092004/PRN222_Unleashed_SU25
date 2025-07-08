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
        public ProviderProfile() {

            CreateMap<Provider, ProviderBareboneDTO>();

        }

        
    }
}
