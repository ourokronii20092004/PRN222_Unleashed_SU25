using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IRoleRepository // : IGenericRepository<Role, int>
    {
        Task<Role?> GetByIdAsync(int id);
        Task<Role?> FindByNameAsync(string roleName);
        Task<List<Role>> GetAllAsync();
        Task<int> SaveChangesAsync();
    }
}