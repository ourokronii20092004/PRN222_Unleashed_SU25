using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IProviderRepository
    {
        Task<List<Provider>> GetAllAsync();
        Task<Provider?> GetByIdAsync(int id);
        Task<Provider> AddAsync(Provider provider);
        Task UpdateAsync(Provider provider);
        Task DeleteAsync(Provider provider);

        Task<bool> ExistsByProviderPhoneAsync(string providerPhone);
        Task<bool> ExistsByProviderEmailAsync(string providerEmail);

        Task<int> SaveChangesAsync();
    }
}