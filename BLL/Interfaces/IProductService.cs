using DAL.DTOs;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    internal interface IProductService
    {
        // Lấy danh sách tất cả sản phẩm
        Task<List<ProductListDTO>> GetAllProductsAsync();

        // Lấy thông tin chi tiết sản phẩm theo ID
        Task<Product?> GetProductByIdAsync(Guid id);

        // Tạo mới một sản phẩm
        Task<Product> CreateProductAsync(ProductDTO productDTO);

        // Cập nhật sản phẩm hiện tại
        Task<Product> UpdateProductAsync(Guid id, ProductDTO productDTO);

        // Kiểm tra xem sản phẩm đã tồn tại chưa
        Task<bool> ProductExistsAsync(Guid id);

        // Thêm các biến thể vào sản phẩm đã tồn tại
        Task<Product?> AddVariationsToExistingProductAsync(Guid productId, List<ProductDTO.ProductVariationDTO> variationDTOs);

        // Lấy danh sách sản phẩm còn trong kho
        Task<List<ProductDetailDTO>> GetProductsInStockAsync();

        // Xóa sản phẩm theo ID
        Task<bool> DeleteProductAsync(Guid id);
    }
}
