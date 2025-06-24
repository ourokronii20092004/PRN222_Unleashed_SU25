using AutoMapper;
using BLL.Services.Interfaces;
using BLL.Utilities;
using DAL.DTOs.UserDTOs;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository accountRepository, IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = accountRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddUserAsync(RegisterUserDTO registerUser, int roleId)
        {
            try
            {
                var user = _mapper.Map<User>(registerUser);
                user.UserPassword = HashingPassword.HashPassword(user.UserPassword);
                user.IsUserEnabled = true;
                user.Role = await _roleRepository.GetByIdAsync(roleId); // Role is passed as a parameter
                user.UserCreatedAt = DateTime.UtcNow;
                user.UserUpdatedAt = DateTime.UtcNow;
                await _userRepository.AddAsync(user);
                return true;
            }
            catch (DBConcurrencyException)
            {             
                return false;
            }
            catch (Exception) 
            {                
                return false;
            }
        }
        public async Task<bool> DeleteUserAsync(string username)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(username, nameof(username));
                var user = await _userRepository.GetByUsernameAsync(username);
                if (user != null)
                {
                    user.IsUserEnabled = false;
                    user.UserUpdatedAt = DateTime.UtcNow;
                    await _userRepository.Update(user);
                    return true;
                }
                return false;
            }
           
            catch (DBConcurrencyException)
            {
                return false;
            } 
            catch (Exception)
            {
                return false;
            }

        }
        public async Task<bool> EditUserAsync(UserDetailDTO user)
        {
            try
            {
                var _user = await _userRepository.GetByUsernameAsync(user.UserUsername);
                if (_user != null)
                {
                    _mapper.Map(user, _user);
                    _user.UserUpdatedAt = DateTime.UtcNow;
                    await _userRepository.Update(_user);
                   return true; 
                }
                return false;
            }
            catch (DbUpdateConcurrencyException)
            {             
                    return false;             
            }
        }

        public async Task<IEnumerable<UserDetailDTO>> GetAccountsAsync()
        {
            IEnumerable<User> users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDetailDTO>>(users);
        }



        public async Task<bool> ValidationUserAsync(string username)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(username);
            return await _userRepository.GetByUsernameAsync(username) != null;
        }

        public async Task<UserDetailDTO> GetUserByUsernameAsync(string username)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(username);
            var user = await _userRepository.GetByUsernameAsync(username);    
            return _mapper.Map<UserDetailDTO>(user);    
        }
    }
}
