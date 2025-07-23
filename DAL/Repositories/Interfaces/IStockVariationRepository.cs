using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IStockVariationRepository
    {
        // For composite keys, getting by ID usually means passing the key components.
        Task<StockVariation?> GetByIdAsync(int variationId, int stockId);
        //Task<StockVariation?> GetByIdAsync(int variationId);
        Task<List<StockVariation>> FindByVariationIdAsync(int variationId);
        Task<int?> GetTotalStockQuantityForVariationAsync(int variationId); // Corresponds to findStockProductByProductVariationId
        Task<int?> GetTotalStockQuantityForProductAsync(System.Guid productId); // Corresponds to getTotalStockQuantityForProduct (productId is Guid)

        Task AddAsync(StockVariation stockVariation);
        Task UpdateAsync(StockVariation stockVariation);
        Task DeleteAsync(StockVariation stockVariation); // Or by IDs: int variationId, int stockId
        Task<List<StockVariation>> GetAllAsync(); // For general JpaRepository.findAll() equivalent

        Task<int> SaveChangesAsync();
    }
}
