using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<User>> GetAccountsAsync();
        Task<bool> AddEmployeeAsync(User user);
        Task<bool> AddCustomerAsync(User user);
        Task<bool> EditUserAsync(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> ValidationUserAsync(string username);

    }
}
