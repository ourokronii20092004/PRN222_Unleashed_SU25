using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IVariationRepository
    {
        Task<List<Variation>> GetAllAsync(); // Standard JpaRepository.findAll()
        Task<Variation?> GetByIdAsync(int id); // Standard JpaRepository.findById()
        Task<Variation> AddAsync(Variation variation);
        Task UpdateAsync(Variation variation);
        Task DeleteAsync(int id);

        // Corresponds to findProductVariationByProductId(@Param("productId") String productId)
        Task<List<Variation>> FindProductVariationByProductIdAsync(Guid productId);

        // Corresponds to findByProduct_ProductCodeAndColor_ColorNameAndSize_SizeName
        Task<Variation?> FindByProductCodeAndColorNameAndSizeNameAsync(string productCode, string colorName, string sizeName);

        // Corresponds to findProductIdByVariationId(@Param("variationId") int variationId)
        Task<Guid?> FindProductIdByVariationIdAsync(int variationId); // ProductId is Guid

        // Corresponds to findProductVariationsByProductIds(@Param("productIds") List<String> productIds)
        Task<List<Variation>> FindProductVariationsByProductIdsAsync(List<Guid> productIds);

        // Corresponds to findProductIdsByVariationIds(@Param("variationIds") List<Integer> variationIds)
        Task<List<Guid>> FindProductIdsByVariationIdsAsync(List<int> variationIds);

        Task<int> SaveChangesAsync();
    }
}