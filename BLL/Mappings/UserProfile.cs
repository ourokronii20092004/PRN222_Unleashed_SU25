using AutoMapper;
using DAL.DTOs.UserDTOs;
using DAL.Models;

namespace BLL.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile() {
            CreateMap<RegisterUserDTO, User>();
            CreateMap<UserDetailDTO, User>()
                .ForMember(u => u.UserCreatedAt, c => c.Ignore())
                .ForMember(u => u.RoleId, opt => opt.Ignore())
                .ForMember(u => u.Role, opt => opt.Ignore())
                .ForMember(u => u.UserGoogleId, opt => opt.Ignore())
                .ForMember(u => u.UserEmail, opt => opt.Ignore())
                .ForMember(u=> u.IsUserEnabled, opt => opt.Ignore())
                .ForMember(u => u.UserImage, opt => opt.Ignore());

            CreateMap<UserLoginDTO, User>();
            
            CreateMap<User,UserDetailDTO>()
                .ForMember(udto => udto.Role, opt => opt.MapFrom(u => u.Role));
        }
    }
}
