﻿using BLL.Services.Interfaces;
using DAL.Data;
using DAL.DTOs.ProductDTOs;
using DAL.Models;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductStatusRepository _productStatusRepository;
        private readonly IVariationRepository _variationRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IColorRepository _colorRepository;

        public ProductService(
            IProductRepository productRepository,
            IProductStatusRepository productStatusRepository,
            IVariationRepository variationRepository,
            ISizeRepository sizeRepository,
            IColorRepository colorRepository)
        {
            _productRepository = productRepository;
            _productStatusRepository = productStatusRepository;
            _variationRepository = variationRepository;
            _sizeRepository = sizeRepository;
            _colorRepository = colorRepository;
        }

        public async Task<Product?> AddVariationsToExistingProductAsync(
     Guid productId, List<ProductDTO.ProductVariationDTO> variationDTOs)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return null;

            var newVariations = new List<Variation>();

            foreach (var dto in variationDTOs)
            {
                var size = await _sizeRepository.GetByIdAsync(dto.SizeId);
                var color = await _colorRepository.GetByIdAsync(dto.ColorId);

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
                
                foreach (var variation in newVariations)
                {
                    await _variationRepository.AddAsync(variation);  
                }

                await _variationRepository.SaveChangesAsync();

                foreach (var variation in newVariations)
                {
                    product.Variations.Add(variation);
                }
            }

            return product;
        }


        public async Task<Product> CreateProductAsync(ProductDTO productDTO)
        {
            // Kiểm tra xem mã sản phẩm đã tồn tại chưa
            var existingProductId = await _productRepository.FindIdByProductCodeAsync(productDTO.ProductCode);
            // Chuyển đổi ProductDTO thành Product và thiết lập các trường cơ bản
            var product = productDTO.ToProduct();
            product.ProductCreatedAt = DateTimeOffset.UtcNow;
            product.ProductUpdatedAt = DateTimeOffset.UtcNow;

            // Thêm sản phẩm vào cơ sở dữ liệu
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            // Thêm danh mục vào sản phẩm nếu có
            if (productDTO.Categories != null && productDTO.Categories.Any())
            {
                foreach (var category in productDTO.Categories)
                {
                    // Thêm CategoryId vào sản phẩm
                    await _productRepository.AddProductCategoryAsync(product.ProductId, category.CategoryId);
                }
            }

            // Tạo danh sách biến thể sản phẩm từ dữ liệu DTO
            var variations = new List<Variation>();
            foreach (var variationDTO in productDTO.Variations)
            {
                // Lấy thông tin Size và Color từ repositories
                var size = await _sizeRepository.GetByIdAsync(variationDTO.SizeId);
                var color = await _colorRepository.GetByIdAsync(variationDTO.ColorId);

                if (size != null && color != null)
                {
                    // Tạo mới biến thể sản phẩm
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
                foreach (var variation in variations)
                {
                    await _variationRepository.AddAsync(variation); 
                }
                await _variationRepository.SaveChangesAsync(); 
            }
          
            product.Variations = variations;

            return product; 
        }


        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
                return false;

            // Xóa tất cả các biến thể của sản phẩm theo ProductId (Guid)
            await _variationRepository.DeleteByProductIdAsync(product.ProductId);

            // Xóa sản phẩm
            await _productRepository.Delete(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductListDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();

            var productListDTOs = new List<ProductListDTO>();

            foreach (var product in products)
            {
                var productDTO = new ProductListDTO
                {
                    ProductCreatedAt = product.ProductCreatedAt,
                    ProductUpdatedAt = product.ProductUpdatedAt,
                    ProductCode = product.ProductCode,
                    ProductStatusName = product.ProductStatus?.ProductStatusName,
                    ProductId = product.ProductId.ToString(),
                    ProductName = product.ProductName,
                    ProductDescription = product.ProductDescription,
                    BrandId = product.BrandId ?? 0,
                    BrandName = product.Brand?.BrandName,
                    CategoryList = product.Categories.Select(c => new Category
                    {
                        CategoryId = c.CategoryId,
                        CategoryName = c.CategoryName
                    }).ToList(),
                    //Variations = await _variationRepository.FindProductVariationByProductIdAsync(product.ProductId)
                };

                productListDTOs.Add(productDTO);
            }

            return productListDTOs;
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<List<ProductDetailDTO>> GetProductsInStockAsync()
        {
            var productsInStock = await _productRepository.FindProductsInStockAsync();

            var productDetailDTOs = new List<ProductDetailDTO>();

            foreach (var product in productsInStock)
            {
                var productDetailDTO = new ProductDetailDTO
                {
                    ProductId = product.ProductId.ToString(),
                    ProductName = product.ProductName,
                    ProductCode = product.ProductCode,
                    ProductDescription = product.ProductDescription,
                    ProductCreatedAt = product.ProductCreatedAt,
                    ProductUpdatedAt = product.ProductUpdatedAt,
                    Brand = product.Brand,
                    ProductStatusId = product.ProductStatusId.HasValue ?
                        await _productStatusRepository.GetByIdAsync(product.ProductStatusId.Value) : null,
                    Categories = product.Categories.Select(c => new Category
                    {
                        CategoryId = c.CategoryId,
                        CategoryName = c.CategoryName
                    }).ToList(),
                    ProductVariations = await _variationRepository.FindProductVariationByProductIdAsync(product.ProductId)
                };

                productDetailDTOs.Add(productDetailDTO);
            }

            return productDetailDTOs;
        }



        public async Task<bool> ProductExistsAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return product != null;
        }

        public async Task<Product> UpdateProductAsync(Guid id, ProductDTO productDTO)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found!");
            }

            // Update basic product fields
            product.ProductName = productDTO.ProductName;
            product.ProductCode = productDTO.ProductCode;
            product.ProductDescription = productDTO.ProductDescription;
            product.ProductStatusId = productDTO.ProductStatusId;
            product.BrandId = productDTO.BrandId;
            product.ProductUpdatedAt = productDTO.UpdatedAt ?? DateTimeOffset.UtcNow;
            product.ProductCreatedAt = productDTO.CreatedAt ?? DateTimeOffset.UtcNow;

            // Handle variations
            await UpdateProductVariations(product, productDTO.Variations);

            // Save changes
            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChangesAsync();

            return product;
        }
        private async Task UpdateProductVariations(Product product, List<ProductDTO.ProductVariationDTO> variationDTOs)
        {
            if (variationDTOs == null) return;

            // Update existing variations
            foreach (var variation in product.Variations.ToList())
            {
                var dto = variationDTOs.FirstOrDefault(v =>
                    v.SizeId == variation.SizeId &&
                    v.ColorId == variation.ColorId);

                if (dto != null)
                {
                    // Update existing variation
                    variation.VariationPrice = dto.ProductPrice ?? 0;
                    variation.VariationImage = dto.ProductVariationImage;
                }
                else
                {
                    // Remove variation not in DTO
                    await _variationRepository.DeleteAsync(variation.VariationId);
                }
            }

            // Add new variations
            foreach (var dto in variationDTOs)
            {
                var exists = product.Variations.Any(v =>
                    v.SizeId == dto.SizeId &&
                    v.ColorId == dto.ColorId);

                if (!exists)
                {
                    var size = await _sizeRepository.GetByIdAsync(dto.SizeId);
                    var color = await _colorRepository.GetByIdAsync(dto.ColorId);

                    if (size != null && color != null)
                    {
                        var newVariation = new Variation
                        {
                            ProductId = product.ProductId,
                            SizeId = size.SizeId,
                            ColorId = color.ColorId,
                            VariationPrice = dto.ProductPrice ?? 0,
                            VariationImage = dto.ProductVariationImage
                        };
                        await _variationRepository.AddAsync(newVariation);
                        product.Variations.Add(newVariation);
                    }
                }
            }

            await _variationRepository.SaveChangesAsync();
        }
        public async Task<(List<ProductSearchResultDTO> Products, int TotalCount)> SearchProductsAsync(string? query, int skip, int take)
        {
            return await _productRepository.SearchProductsAsync(query, skip, take);
        }

        public async Task<Product?> GetProductByCodeAsync(string productCode)
        {
            return await _productRepository.GetProductByCodeAsync(productCode);
        }
    }
}
