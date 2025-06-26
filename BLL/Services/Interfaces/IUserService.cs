using DAL.DTOs.UserDTOs;
using Microsoft.AspNetCore.Http;

namespace BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDetailDTO>> GetAccountsAsync();
        Task<bool> AddUserAsync(RegisterUserDTO user, int RoleId, IFormFile? image);
        Task<bool> EditUserAsync(UserDetailDTO user, IFormFile? image);
        Task<bool> DeleteUserAsync(string username);
        Task<UserDetailDTO?> GetUserByUsernameAsync(string username);
        Task<bool> ValidationUserAsync(string username);

    }
}
