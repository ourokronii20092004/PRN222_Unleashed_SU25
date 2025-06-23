using BLL.Interfaces;
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
    internal class ProductService : IProductService
    {
        private readonly UnleashedContext _context;
        public async Task<Product?> AddVariationsToExistingProductAsync(
     string productId, List<ProductDTO.ProductVariationDTO> variationDTOs)
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


        public Task CreateBrandAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product?> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductDetailDTO>> GetProductsInStockAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> ProductExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
