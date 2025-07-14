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
            CreateMap<Cart, CartDTO>();
        }
        
    }
}
