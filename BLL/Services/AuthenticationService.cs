using AutoMapper;
using BLL.Services.Interfaces;
using BLL.Utilities;
using DAL.DTOs.UserDTOs;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public AuthenticationService(IUserRepository userRepository,IMapper mapper ) 
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Task<bool> ForgotPassword(UserLoginDTO loginInfor)
        {
            throw new NotImplementedException();
        }

        public async Task<UserDetailDTO?> Login(UserLoginDTO loginInfor)
        {
           var user = await _userRepository.GetByUsernameAsync(loginInfor.UserUsername);
            if (user != null && user.IsUserEnabled.GetValueOrDefault(false)) 
            return HashingPassword.VerifyPassword(loginInfor.UserPassword,user.UserPassword) ? _mapper.Map<UserDetailDTO>(user) : null;     
            return null;
        }

        public Task<bool> Register(UserLoginDTO loginInfor)
        {
            throw new NotImplementedException();
        }

    }
}
