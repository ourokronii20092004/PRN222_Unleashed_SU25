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
    string productIdString, List<ProductDTO.ProductVariationDTO> variationDTOs)
        {
            if (!Guid.TryParse(productIdString, out Guid productIdAsGuid))
            {
                // Invalid Product ID format
                return null;
            }

            var product = await _context.Products
                .Include(p => p.Variations)
                .FirstOrDefaultAsync(p => p.ProductId == productIdAsGuid);

            if (product == null)
            {
                // Product not found
                return null;
            }

            var newVariations = new List<Variation>();

            foreach (var dto in variationDTOs)
            {
                var size = await _context.Sizes.FindAsync(dto.SizeId);
                var color = await _context.Colors.FindAsync(dto.ColorId);

                if (size == null || color == null)
                {
                    // Size or Color not found for a DTO, skipping this variation
                    continue;
                }

                bool exists = product.Variations.Any(v =>
                    v.SizeId == size.SizeId && v.ColorId == color.ColorId);

                if (!exists)
                {
                    var variation = new Variation
                    {
                        ProductId = product.ProductId, // Use the Guid from the fetched product
                        SizeId = size.SizeId,
                        ColorId = color.ColorId,
                        VariationPrice = dto.ProductPrice ?? 0, // Assuming ProductPrice is decimal? or double?
                        VariationImage = dto.ProductVariationImage,
                        // VariationCreatedAt, VariationUpdatedAt could be set here if needed
                        // VariationCreatedAt = DateTimeOffset.UtcNow,
                        // VariationUpdatedAt = DateTimeOffset.UtcNow
                    };
                    newVariations.Add(variation);
                }
            }

            if (newVariations.Any())
            {
                await _context.Variations.AddRangeAsync(newVariations);
                // Potentially, product's updated timestamp should be set here if applicable
                // product.ProductUpdatedAt = DateTimeOffset.UtcNow;
                // _context.Update(product); // Mark product as modified if its properties (like UpdatedAt) changed
                await _context.SaveChangesAsync();

                // After SaveChanges, EF Core usually updates navigation properties if tracking is enabled.
                // The explicit adding to product.Variations collection might be redundant
                // if the relationship is correctly configured and newVariations are tracked.
                // However, if you need the in-memory 'product' object to reflect these immediately
                // without re-querying, this can be useful.

                // The check for Collection<Variation> vs ICollection<Variation> is a bit unusual.
                // Typically product.Variations would be ICollection<Variation> and Add would work directly.
                // If it's truly a fixed Collection<T> that doesn't allow adding after instantiation,
                // then the product.Variations property might need to be initialized differently in the Product model.
                // Assuming product.Variations is initialized as `new List<Variation>()` or `new HashSet<Variation>()`
                // the direct `product.Variations.Add(variation)` within a loop (or just relying on EF Core to fix up) is standard.

                // Simpler way if product.Variations is a standard ICollection<T> initialized (e.g., new List<Variation>()):
                // foreach (var newVariation in newVariations)
                // {
                //     product.Variations.Add(newVariation); // EF Core will fix up navigation if tracked
                // }
                // Or often, this manual addition to the in-memory collection is not strictly needed
                // if the client will re-fetch or if the `product` instance returned is understood to be
                // potentially stale regarding its collections until re-queried or explicitly updated.
            }

            return product; // This product instance might not have the new variations in its .Variations collection
                            // unless EF Core fix-up happened or you manually added them and the collection type supports it.
                            // For a fully updated product object including new variations, re-querying after save
                            // or ensuring the collection is correctly updated is best.
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
