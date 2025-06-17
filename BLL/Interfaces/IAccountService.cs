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
        Task<List<User>> GetAccountsAsync();
        Task<bool> AddUser(User user);
        Task<bool> EditUser(User user);
        Task<bool> DeleteUser(User user);
        Task<User?> GetUserByUsername(string username);
        Task<bool> ValidationUser(string username);

    }
}
