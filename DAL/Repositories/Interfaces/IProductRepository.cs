using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTO;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product, Guid>
    {
        Task<bool> ExistsByBrandAsync(int brandId);
        Task<ProductStatus?> GetProductStatusByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<string?> GetProductNameByVariationIdAsync(int variationId);
        Task<bool> ExistsByCategoryIdAsync(int categoryId);
        Task SoftDeleteProductAsync(Guid id);
        Task AddProductCategoryAsync(Guid productId, int categoryId);
        Task<Product?> FindProductWithVariationsAsync(Guid productId);
        Task<(List<ProductSearchResultDTO> Products, int TotalCount)> SearchProductsAsync(string? query, int skip, int take);
        Task<List<Product>> FindByProductIdInAsync(List<Guid> productIds);
        Task<Guid?> FindIdByProductCodeAsync(string productCode);
        Task<List<Product>> FindProductsInStockAsync();
        Task<List<Guid>> FindIdsByProductCodesAsync(List<string> productCodes);
        Task<List<Product>> FindAllActiveProductsAsync();
        Task<string?> FindNameByProductCodeAsync(string productCode);
        Task<Dictionary<Guid, List<string>>> GetCategoryNamesMapByProductIdsAsync(List<Guid> productIds);
        // This might be redundant if IGenericRepository handles unit of work
        // or if individual methods in the implementation save changes.
        Task<int> SaveChangesAsync();
        // However, keeping it allows the service layer to control transactions explicitly.
        // So in conclusion, IDK. 💀
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        
    }
}