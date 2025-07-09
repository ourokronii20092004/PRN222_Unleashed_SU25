using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTOs.ProductDTOs;
using DAL.Models;

namespace DAL.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product, Guid>
    {
        new Task<List<Product>> GetAllAsync();
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
        Task<int> SaveChangesAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task<Product> AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task<List<Product>> GetProductsWithVariationsAsync();
        Task<List<ProductImportSelectionDTO>> GetProductsForImportSelectionAsync(int stockId);
        Task<Product?> GetProductByCodeAsync(string productCode);
        Task<List<Product>> GetAllWithPagingAsync(int skip, int take);
        Task<int> CountAllProductsAsync();
        Task<int> CountSearchResultsAsync(string query);
        Task<List<Product>> GetProductsWithPagingAsync(int skip, int take, string query);
        Task<int> GetProductsCountAsync(string query);
    }

}