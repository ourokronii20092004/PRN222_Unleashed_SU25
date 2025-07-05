using DAL.DTOs.ProductDTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductListDTO>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<Product> CreateProductAsync(ProductDTO productDTO);
        Task<Product> UpdateProductAsync(Guid id, ProductDTO productDTO);
        Task<bool> ProductExistsAsync(Guid id);
        Task<Product?> AddVariationsToExistingProductAsync(Guid productId, List<ProductDTO.ProductVariationDTO> variationDTOs);
        Task<List<ProductDetailDTO>> GetProductsInStockAsync();
        Task<bool> DeleteProductAsync(Guid id);
        Task<(List<ProductSearchResultDTO> Products, int TotalCount)> SearchProductsAsync(string? query, int skip, int take);
        Task<Product?> GetProductByCodeAsync(string productCode);
    }
}
