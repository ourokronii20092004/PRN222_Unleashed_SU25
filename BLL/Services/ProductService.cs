using AutoMapper;
using BLL.Services.Interfaces;
using DAL.DTOs.ProductDTOs;
using DAL.Models;
using DAL.Repositories;
using DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
        private readonly IBrandRepository _brandRepository;
        
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        
        
        public ProductService(
            IProductRepository productRepository,
            IProductStatusRepository productStatusRepository,
            IVariationRepository variationRepository,
            ISizeRepository sizeRepository,
            IColorRepository colorRepository,
            IMapper mapper,
            ILogger<ProductService> logger,
            IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _productStatusRepository = productStatusRepository;
            _variationRepository = variationRepository;
            _sizeRepository = sizeRepository;
            _colorRepository = colorRepository;
            _brandRepository = brandRepository;
            _mapper = mapper;
            _logger = logger;
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
                        VariationImage = dto.ProductVariationImageUrl,
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
            var product = productDTO.ToProduct();
            product.ProductCreatedAt = DateTimeOffset.UtcNow;
            product.ProductUpdatedAt = DateTimeOffset.UtcNow;

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            if (productDTO.Categories != null && productDTO.Categories.Any())
            {
                foreach (var category in productDTO.Categories)
                {
                    await _productRepository.AddProductCategoryAsync(product.ProductId, category.CategoryId);
                }
            }
            
            var variations = new List<Variation>();
            foreach (var variationDTO in productDTO.Variations)
            {
                var size = await _sizeRepository.GetByIdAsync(variationDTO.SizeId);
                var color = await _colorRepository.GetByIdAsync(variationDTO.ColorId);

                if (size != null && color != null)
                {
                    var variation = new Variation
                    {
                        ProductId = product.ProductId,
                        SizeId = size.SizeId,
                        ColorId = color.ColorId,
                        VariationPrice = variationDTO.ProductPrice ?? 0,
                        VariationImage = variationDTO.ProductVariationImageUrl
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
            await _variationRepository.DeleteByProductIdAsync(product.ProductId);
            await _productRepository.Delete(product);
            await _productRepository.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductListDTO>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var variations = await _variationRepository.GetAllAsync();
            var variationsByProductId = variations
                .GroupBy(v => v.ProductId)
                .ToDictionary(g => g.Key, g => g.ToList());
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
                };
                if (variationsByProductId.ContainsKey(product.ProductId))
                {
                    productDTO.Variations = variationsByProductId[product.ProductId]
                        .Select(v => new ProductListDTO.ProductVariationDTO
                        {
                            SizeId = v.SizeId,
                            ColorId = v.ColorId,
                            ProductPrice = v.VariationPrice,
                            ProductVariationImage = v.VariationImage
                        }).ToList();
                }
                productListDTOs.Add(productDTO);
            }
            return productListDTOs;
        }

        public async Task<Product?> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product?.Variations != null)
            {
                foreach (var variation in product.Variations)
                {
                    if (variation.SizeId != null && variation.Size == null)
                        variation.Size = await _sizeRepository.GetByIdAsync(variation.SizeId);

                    if (variation.ColorId != null && variation.Color == null)
                        variation.Color = await _colorRepository.GetByIdAsync(variation.ColorId);
                }
            }

            return product;
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
            product.ProductName = productDTO.ProductName;
            product.ProductCode = productDTO.ProductCode;
            product.ProductDescription = productDTO.ProductDescription;
            product.ProductStatusId = productDTO.ProductStatusId;
            product.BrandId = productDTO.BrandId;
            product.ProductUpdatedAt = productDTO.UpdatedAt ?? DateTimeOffset.UtcNow;
            product.ProductCreatedAt = productDTO.CreatedAt ?? DateTimeOffset.UtcNow;

            await UpdateProductVariations(product, productDTO.Variations);
            await _productRepository.UpdateAsync(product);
            await _productRepository.SaveChangesAsync();
            return product;
        }

        private async Task UpdateProductVariations(Product product, List<ProductDTO.ProductVariationDTO> variationDTOs)
        {
            if (variationDTOs == null) return;
            
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
                            VariationImage = dto.ProductVariationImageUrl
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

        public async Task<List<ProductListDTO>> GetAllProductsAsync(int skip, int take)
        {
            var products = await _productRepository.GetAllWithPagingAsync(skip, take);
            var variations = await _variationRepository.GetAllAsync();
            var variationsByProductId = variations
                .GroupBy(v => v.ProductId)
                .ToDictionary(g => g.Key, g => g.ToList());
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
                    CategoryList = product.Categories?.Select(c => new Category
                    {
                        CategoryId = c.CategoryId,
                        CategoryName = c.CategoryName
                    }).ToList() ?? new List<Category>(),
                };
                if (variationsByProductId.TryGetValue(product.ProductId, out var productVariations))
                {
                    productDTO.Variations = productVariations
                        .Select(v => new ProductListDTO.ProductVariationDTO
                        {
                            SizeId = v.SizeId,
                            ColorId = v.ColorId,
                            ProductPrice = v.VariationPrice,
                            ProductVariationImage = v.VariationImage
                        }).ToList();
                }
                else
                {
                    productDTO.Variations = new List<ProductListDTO.ProductVariationDTO>();
                }
                productListDTOs.Add(productDTO);
            }
            return productListDTOs;
        }

        public async Task<int> CountAllProductsAsync()
        {
            return await _productRepository.CountAllProductsAsync();
        }

        public async Task<int> CountSearchResultsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return await CountAllProductsAsync();
            return await _productRepository.CountSearchResultsAsync(query);
        }


        public async Task<List<ProductImportSelectionDTO>> GetProductsForImportSelectionAsync(int stockId)
        {
            try
            {
                return await _productRepository.GetProductsForImportSelectionAsync(stockId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fetch import select error");
                throw;
            }
        }

        public async Task<ProductDropdownsDTO> GetProductDropdownsAsync()
        {
            var brands = await _brandRepository.GetAllAsync();
            var statuses = await _productStatusRepository.GetAllAsync();
            var sizes = await _sizeRepository.GetAllAsync();
            var colors = await _colorRepository.GetAllAsync();

            return new ProductDropdownsDTO
            {
                Brands = brands.ToList(),
                Statuses = statuses.ToList(),
                Sizes = sizes.ToList(),
                Colors = colors.ToList()
            };
        }
        public async Task<bool> SoftDeleteProductAsync(Guid productId)
        {
            await _productRepository.SoftDeleteProductAsync(productId);
            return true;
        }
    }
}