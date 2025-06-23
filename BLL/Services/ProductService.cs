using BLL.Services.Interfaces;
using DAL.Data;
using DAL.DTOs;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly UnleashedContext _context;
        public async Task<Product?> AddVariationsToExistingProductAsync(
     Guid productId, List<ProductDTO.ProductVariationDTO> variationDTOs)
        {
            var product = await _context.Products
                .Include(p => p.Variations)
                .FirstOrDefaultAsync(p => p.ProductId == Guid.Parse(productId));

            if (product == null)
                return null;

            var newVariations = new List<Variation>();

            foreach (var dto in variationDTOs)
            {
                var size = await _context.Sizes.FindAsync(dto.SizeId);
                var color = await _context.Colors.FindAsync(dto.ColorId);

                if (size == null || color == null)
                    continue;

                bool exists = product.Variations.Any(v =>
                    v.SizeId == size.SizeId && v.ColorId == color.ColorId);

                if (!exists)
                {
                    var variation = new Variation
                    {
                        ProductId = product.ProductId,
                        SizeId = size.SizeId,
                        ColorId = color.ColorId,
                        VariationPrice = dto.ProductPrice ?? 0,
                        VariationImage = dto.ProductVariationImage,
                    };

                    newVariations.Add(variation);
                }
            }

            if (newVariations.Any())
            {
                await _context.Variations.AddRangeAsync(newVariations);
                await _context.SaveChangesAsync();

                if (product.Variations is Collection<Variation> variationsCollection)
                {
                    foreach (var variation in newVariations)
                    {
                        variationsCollection.Add(variation);
                    }
                }
                else
                {
                    foreach (var variation in newVariations)
                    {
                        product.Variations.Add(variation);
                    }
                }
            }

            return product;
        }


        // Tạo sản phẩm mới
        public async Task<Product> CreateProductAsync(ProductDTO productDTO)
        {
            // Kiểm tra xem ProductCode đã tồn tại chưa
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductCode == productDTO.ProductCode);

            if (existingProduct != null)
            {
                throw new InvalidOperationException("ProductCode already exists!");
            }

            var product = productDTO.ToProduct();
            product.ProductCreatedAt = DateTimeOffset.UtcNow;
            product.ProductUpdatedAt = DateTimeOffset.UtcNow;

            // Lưu sản phẩm vào cơ sở dữ liệu
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Thêm danh mục vào sản phẩm nếu có
            if (productDTO.Categories != null && productDTO.Categories.Any())
            {
                foreach (var categoryId in productDTO.Categories)
                {
                    var category = await _context.Categories.FindAsync(categoryId);
                    if (category != null)
                    {
                        product.Categories.Add(category);
                    }
                }
            }

            // Thêm biến thể sản phẩm nếu có
            var variations = new List<Variation>();
            foreach (var variationDTO in productDTO.Variations)
            {
                var size = await _context.Sizes.FindAsync(variationDTO.SizeId);
                var color = await _context.Colors.FindAsync(variationDTO.ColorId);

                if (size != null && color != null)
                {
                    var variation = new Variation
                    {
                        ProductId = product.ProductId,
                        SizeId = size.SizeId,
                        ColorId = color.ColorId,
                        VariationPrice = variationDTO.ProductPrice ?? 0,
                        VariationImage = variationDTO.ProductVariationImage
                    };

                    variations.Add(variation);
                }
            }

            if (variations.Any())
            {
                await _context.Variations.AddRangeAsync(variations);
                await _context.SaveChangesAsync();
            }

            product.Variations = variations; // Liên kết biến thể với sản phẩm
            return product;
        }



        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _context.Products
                .Include(p => p.Variations)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return false;

            // Delete related variations
            _context.Variations.RemoveRange(product.Variations);

            // Delete product
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<List<ProductListDTO>> GetAllProductsAsync()
        {
            var result = await _context.Products
                .Where(p => p.ProductStatusId != null)  // Lọc sản phẩm đang hoạt động
                .ToListAsync();

            var productList = new List<ProductListDTO>();

            foreach (var product in result)
            {
                string productId = product.ProductId.ToString();

                // Kiểm tra xem sản phẩm đã có trong danh sách chưa
                bool exists = productList.Any(dto => dto.ProductId == productId);

                // Bỏ qua nếu sản phẩm đã có hoặc trạng thái bị xóa (ProductStatus null)
                if (exists || product.ProductStatusId == null)
                {
                    continue;
                }

                var productListDTO = new ProductListDTO
                {
                    ProductId = productId,
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    BrandId = product.BrandId ?? 0,
                    BrandName = product.Brand?.BrandName
                };

                // Lấy danh mục của sản phẩm
                productListDTO.CategoryList = product.Categories.Select(c => new Category
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryName
                }).ToList();

                // Lấy biến thể sản phẩm đầu tiên cho giá và hình ảnh
                var firstVariation = await _context.Variations
                    .Where(v => v.ProductId == product.ProductId)
                    .FirstOrDefaultAsync();

                if (firstVariation != null)
                {
                    productListDTO.ProductPrice = firstVariation.VariationPrice ?? 0;
                    productListDTO.ProductVariationImage = firstVariation.VariationImage;
                }

                // Lấy thông tin khuyến mãi
                var sale = await _context.Sales
                   .Where(s => s.Products.Any(p => p.ProductId == product.ProductId) && s.SaleStatus.SaleStatusName == "ACTIVE")
                   .FirstOrDefaultAsync();

                if (sale != null)
                {
                    productListDTO.Sale = sale;
                    productListDTO.SaleValue = sale.SaleValue;
                }

                // Lấy đánh giá của sản phẩm (tổng số và điểm trung bình)
                var ratingData = await _context.Reviews
                    .Where(r => r.ProductId == product.ProductId)
                    .GroupBy(r => r.ProductId)
                    .Select(g => new
                    {
                        TotalRatings = g.Count(),
                        AverageRating = g.Average(r => r.ReviewRating ?? 0)
                    })
                    .FirstOrDefaultAsync();

                if (ratingData != null)
                {
                    productListDTO.TotalRatings = ratingData.TotalRatings;
                    productListDTO.AverageRating = Math.Round(ratingData.AverageRating, 2);
                }
                else
                {
                    productListDTO.TotalRatings = 0;
                    productListDTO.AverageRating = 0.0;
                }

                // Tính tổng số lượng sản phẩm
                var totalQuantity = await _context.StockVariations
                    .Where(sv => sv.StockId.ToString() == product.ProductId.ToString())
                    .SumAsync(sv => sv.StockQuantity);

                productListDTO.Quantity = totalQuantity ?? 0;

                // Thêm vào danh sách sản phẩm
                productList.Add(productListDTO);
            }

            return productList;
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _context.Products
     .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public Task<List<ProductDetailDTO>> GetProductsInStockAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ProductExistsAsync(Guid id)
        {
            return await _context.Products.AnyAsync(p => p.ProductId == id);
        }

        // Cập nhật thông tin sản phẩm
        public async Task<Product> UpdateProductAsync(Guid id, ProductDTO productDTO)
        {
            var product = await _context.Products
                .Include(p => p.Categories)
                .Include(p => p.Variations)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                throw new InvalidOperationException("Product not found!");
            }

            product.ProductName = productDTO.ProductName;
            product.ProductCode = productDTO.ProductCode;
            product.ProductDescription = productDTO.ProductDescription;
            product.ProductStatusId = productDTO.ProductStatusId;

            // Cập nhật danh mục
            if (productDTO.Categories != null && productDTO.Categories.Any())
            {
                foreach (var categoryId in productDTO.Categories)
                {
                    var category = await _context.Categories.FindAsync(categoryId);
                    if (category != null)
                    {
                        product.Categories.Add(category); // Thêm đối tượng Category vào sản phẩm
                    }
                }
            }

            // Cập nhật biến thể sản phẩm
            foreach (var variationDTO in productDTO.Variations)
            {
                var size = await _context.Sizes.FindAsync(variationDTO.SizeId);
                var color = await _context.Colors.FindAsync(variationDTO.ColorId);

                if (size != null && color != null)
                {
                    var existingVariation = product.Variations
                        .FirstOrDefault(v => v.SizeId == size.SizeId && v.ColorId == color.ColorId);

                    if (existingVariation != null)
                    {
                        existingVariation.VariationPrice = variationDTO.ProductPrice ?? 0;
                        existingVariation.VariationImage = variationDTO.ProductVariationImage;
                    }
                    else
                    {
                        var newVariation = new Variation
                        {
                            ProductId = product.ProductId,
                            SizeId = size.SizeId,
                            ColorId = color.ColorId,
                            VariationPrice = variationDTO.ProductPrice ?? 0,
                            VariationImage = variationDTO.ProductVariationImage
                        };
                        product.Variations.Add(newVariation);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return product;
        }


    }
}
