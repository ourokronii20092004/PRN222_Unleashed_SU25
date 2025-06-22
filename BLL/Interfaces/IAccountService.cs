using DAL.DTOs.AccountDTOs;
using DAL.Models;

namespace BLL.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDetailDTO>> GetAccountsAsync();
        Task<bool> AddUserAsync(User user, int RoleId);
        Task<bool> EditUserAsync(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> ValidationUserAsync(string username);

    }
}
