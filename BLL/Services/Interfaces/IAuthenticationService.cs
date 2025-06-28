using DAL.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserDetailDTO?> Login(UserLoginDTO loginInfor);
        Task<bool> ForgotPassword(UserLoginDTO loginInfor);
        Task<bool> Register(UserLoginDTO loginInfor);
    }
}
