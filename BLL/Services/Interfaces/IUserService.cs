using DAL.DTOs.UserDTOs;
using DAL.Models;

namespace BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDetailDTO>> GetAccountsAsync();
        Task<bool> AddUserAsync(RegisterUserDTO user, int RoleId);
        Task<bool> EditUserAsync(UserDetailDTO user);
        Task<bool> DeleteUserAsync(string username);
        Task<UserDetailDTO> GetUserByUsernameAsync(string username);
        Task<bool> ValidationUserAsync(string username);

    }
}
