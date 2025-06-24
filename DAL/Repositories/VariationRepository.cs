using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    // Changed from internal to public
    public class VariationRepository : IVariationRepository
    {
        private readonly UnleashedContext _context;
        public VariationRepository(UnleashedContext context)
        {
            _context = context;
        }

        // --- Existing Methods (adjusted signatures/return types where needed) ---

        // Add a new Variation to the database - Changed to return the entity as per typical AddAsync
        public async Task<Variation> AddAsync(Variation entity) // Removed CancellationToken for now to match interface
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _context.Variations.AddAsync(entity);
            // await _context.SaveChangesAsync(); // Will be handled by SaveChangesAsync() or service
            return entity;
        }

        // Delete a Variation from the database - Changed to take id
        public async Task DeleteAsync(int id) // Renamed from Delete, takes id
        {
            var entity = await GetByIdAsync(id); // Fetch first
            if (entity != null)
            {
                _context.Variations.Remove(entity);
                // await _context.SaveChangesAsync();
            }
        }

        // Find variations based on a predicate (filter condition) - Kept as is, not in final IVariationRepository
        public async Task<IEnumerable<Variation>> FindAsync(Expression<Func<Variation, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Variations
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        // Get all variations - Changed to return List<Variation>
        public async Task<List<Variation>> GetAllAsync()
        {
            return await _context.Variations.ToListAsync();
        }

        // Get a variation by its ID - Changed to return Variation?
        public async Task<Variation?> GetByIdAsync(int id) // Removed CancellationToken for now
        {
            // Original: return await _context.Variations.AsNoTracking().FirstOrDefaultAsync(x => x.VariationId == id) ??
            //       throw new ArgumentException("Variation not found", nameof(id));
            // Changed to not throw and return nullable
            return await _context.Variations.AsNoTracking().FirstOrDefaultAsync(x => x.VariationId == id);
        }

        // Update an existing variation - Changed return type to Task
        public async Task UpdateAsync(Variation entity) // Renamed from Update
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.Variations.Update(entity);
            // await _context.SaveChangesAsync();
        }


        // --- Methods from Java VariationRepository (to be added) ---

        public async Task<List<Variation>> FindProductVariationByProductIdAsync(Guid productId)
        {
            // Java:
            // @Query("""
            // SELECT v
            // FROM Variation v
            // LEFT JOIN StockVariation sv ON sv.id.variationId = v.id
            // WHERE v.product.productId = :productId
            //   AND (sv.stockQuantity IS NULL OR sv.stockQuantity <> -1)
            // """)
            // The condition "sv.stockQuantity <> -1" implies -1 is a special value for "not available" or similar.
            // If -1 simply means out of stock and should be included, the condition would differ.
            // Assuming -1 means "do not show this variation at all".
            return await _context.Variations
                .Include(v => v.StockVariations) // Need StockVariations to check quantity
                .Where(v => v.ProductId == productId &&
                             (!v.StockVariations.Any() || v.StockVariations.All(sv => sv.StockQuantity != -1)))
                // The above LINQ means:
                // - variation's product ID matches
                // - AND (either the variation has no stock records OR all its stock records don't have quantity -1)
                // If you want to include variations that have some stock records with qty -1 but others not, this logic needs refinement.
                // A more direct translation of "sv.stockQuantity IS NULL OR sv.stockQuantity <> -1" might involve a left join approach if not all variations have stockvariation records
                // Or, if every variation is expected to have at least one stock_variation record (or none means not in stock for this filter):
                // .Where(v => v.ProductId == productId)
                // .Where(v => !v.StockVariations.Any(sv => sv.StockQuantity == -1)) // if any stock is -1, exclude
                .ToListAsync();
        }

        public async Task<Variation?> FindByProductCodeAndColorNameAndSizeNameAsync(string productCode, string colorName, string sizeName)
        {
            return await _context.Variations
                .Include(v => v.Product)
                .Include(v => v.Color)
                .Include(v => v.Size)
                .FirstOrDefaultAsync(v => v.Product != null && v.Product.ProductCode == productCode &&
                                         v.Color != null && v.Color.ColorName == colorName &&
                                         v.Size != null && v.Size.SizeName == sizeName);
        }

        public async Task<Guid?> FindProductIdByVariationIdAsync(int variationId)
        {
            return await _context.Variations
                .Where(v => v.VariationId == variationId)
                .Select(v => (Guid?)v.ProductId) // Cast to Guid? for FirstOrDefaultAsync
                .FirstOrDefaultAsync();
        }

        public async Task<List<Variation>> FindProductVariationsByProductIdsAsync(List<Guid> productIds)
        {
            if (productIds == null || !productIds.Any())
                return new List<Variation>();

            return await _context.Variations
                .Where(v => productIds.Contains(v.ProductId))
                .ToListAsync();
        }

        public async Task<List<Guid>> FindProductIdsByVariationIdsAsync(List<int> variationIds)
        {
            if (variationIds == null || !variationIds.Any())
                return new List<Guid>();

            return await _context.Variations
                .Where(v => variationIds.Contains(v.VariationId))
                .Select(v => v.ProductId) // ProductId is already Guid
                .Distinct()
                .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // --- Helper methods from your existing class, kept for compatibility if used elsewhere ---
        public async Task<IEnumerable<Variation>> GetVariationsByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.Variations
                .Where(v => v.ProductId == productId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Variation>> GetVariationsByColorIdAsync(int colorId, CancellationToken cancellationToken = default)
        {
            return await _context.Variations
                .Where(v => v.ColorId == colorId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Variation>> GetVariationsBySizeIdAsync(int sizeId, CancellationToken cancellationToken = default)
        {
            return await _context.Variations
                .Where(v => v.SizeId == sizeId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Variation> GetVariationByProductIdAsync(Guid productID, CancellationToken cancellationToken = default) // Should this return list or first?
        {
            return await _context.Variations
                       .FirstOrDefaultAsync(v => v.ProductId == productID, cancellationToken) ??
                   throw new ArgumentException("Variation not found for the given productId", nameof(productID));
        }

        public async Task DeleteByProductIdAsync(Guid productId)
        {
            var variationsToDelete = await _context.Variations
                                       .Where(v => v.ProductId == productId)
                                       .ToListAsync();

            if (variationsToDelete.Any())
            {
                _context.Variations.RemoveRange(variationsToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
