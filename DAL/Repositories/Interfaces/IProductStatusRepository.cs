using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IProductStatusRepository
    {
        Task<List<ProductStatus>> GetAllAsync();
        Task<ProductStatus?> GetByIdAsync(int id);
        Task<ProductStatus?> FindByNameAsync(string name);

        // Corresponds to findStatusByProductId(@Param("productId") String productId)
        Task<int?> FindStatusIdByProductIdAsync(Guid productId);

        Task<int> SaveChangesAsync();
    }
}
