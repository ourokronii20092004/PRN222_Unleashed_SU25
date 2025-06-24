using AutoMapper;
using DAL.DTOs.UserDTOs;
using DAL.Models;

namespace BLL.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile() {
            CreateMap<RegisterUserDTO, User>();
            CreateMap<UserDetailDTO,User>();
            
            CreateMap<User,UserDetailDTO>();
        }
    }
}
