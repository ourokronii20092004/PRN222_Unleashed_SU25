using DAL.DTOs;
using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    internal interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task CreateBrandAsync(Product product);
        Task<bool> UpdateProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> ProductExistsAsync(int id);
        Task<Product?> AddVariationsToExistingProductAsync(string productId, List<ProductDTO.ProductVariationDTO> variationDTOs);
        Task<List<ProductDetailDTO>> GetProductsInStockAsync();
    }
}
