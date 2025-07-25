﻿using DAL.DTOs.ProductDTOs;
using DAL.Models;
using DAL.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Models.ViewModels;

namespace BLL.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductListDTO>> GetAllProductsCustomerAsync();
        Task<Product?> GetProductByIdAsync(Guid id);
        Task<Product> CreateProductAsync(ProductDTO productDTO);
        Task<Product> UpdateProductAsync(Guid id, ProductDTO productDTO);
        Task<bool> ProductExistsAsync(Guid id);
        Task<Product?> AddVariationsToExistingProductAsync(Guid productId, List<ProductDTO.ProductVariationDTO> variationDTOs);
        Task<List<ProductDetailDTO>> GetProductsInStockAsync();
        Task<bool> DeleteProductAsync(Guid id);
        Task<(List<ProductSearchResultDTO> Products, int TotalCount)> SearchProductsAsync(string? query, int skip, int take);
        Task<Product?> GetProductByCodeAsync(string productCode);
        Task<List<ProductListDTO>> GetAllProductsAsync(int skip, int take);
        Task<int> CountAllProductsAsync();  
        Task<int> CountSearchResultsAsync(string query);
        Task<List<ProductImportSelectionDTO>> GetProductsForImportSelectionAsync(int stockId);
        Task<ProductDropdownsDTO> GetProductDropdownsAsync();
        Task<bool> SoftDeleteProductAsync(Guid productId);
        Task<DAL.Models.PagedResult<ProductListDTO>> GetProductsWithPagingAsync(int page, int pageSize, string query);
        Task<DAL.Models.PagedResult<ProductListDTO>> GetProductsWithPagingHomePageAsync(int page, int pageSize, string query);


    }
}

