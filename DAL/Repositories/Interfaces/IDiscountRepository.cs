using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IDiscountRepository
    {
        Task<List<Discount>> GetAllAsync();
        Task<Discount?> GetByIdAsync(int id);
        Task<Discount> AddAsync(Discount discount);
        Task UpdateAsync(Discount discount);
        Task DeleteAsync(Discount discount);
        Task<bool> CodeExistsAsync(string code, int? excludeId = null);
        Task<int> SaveChangesAsync();

        Task<List<DiscountStatus>> GetAllStatusesAsync();
        Task<List<DiscountType>> GetAllTypesAsync();
    }
}
