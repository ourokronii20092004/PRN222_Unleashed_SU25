using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IDiscountService
    {
        Task<List<Discount>> GetAllDiscountsAsync();
        Task<Discount?> GetDiscountByIdAsync(int id); // Mới
        Task CreateDiscountAsync(Discount discount);   // Mới
        Task<bool> UpdateDiscountAsync(Discount discount); // Mới
        Task<bool> DeleteDiscountAsync(int id);      // Mới
    }
}
