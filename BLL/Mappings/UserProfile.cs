using AutoMapper;
using DAL.DTOs.UserDTOs;
using DAL.Models;

namespace BLL.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile() {
            CreateMap<RegisterUserDTO, User>();
            CreateMap<UserDetailDTO,User>()
                .ForMember(u => u.UserCreatedAt, c => c.Ignore());
            CreateMap<UserLoginDTO, User>();
            
            CreateMap<User,UserDetailDTO>();
        }
    }
}
