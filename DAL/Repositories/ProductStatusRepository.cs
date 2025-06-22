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
    public class ProductStatusRepository : IProductStatusRepository // Changed from internal
    {
        private readonly UnleashedContext _context;

        public ProductStatusRepository(UnleashedContext context)
        {
            _context = context;
        }

        // --- Methods from IProductStatusRepository ---
        public async Task<List<ProductStatus>> GetAllAsync()
        {
            return await _context.ProductStatuses.OrderBy(ps => ps.ProductStatusName).ToListAsync();
        }

        public async Task<ProductStatus?> GetByIdAsync(int id)
        {
            // Original from your code:
            // return await _context.ProductStatuses.AsNoTracking().FirstOrDefaultAsync(x => x.ProductStatusId == id) ??
            //     throw new ArgumentException("ProductStatus not found", nameof(id));
            // Changed to return nullable without throwing from repo:
            return await _context.ProductStatuses.FindAsync(id);
        }

        public async Task<ProductStatus?> FindByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            string normalizedName = name.ToLower();
            return await _context.ProductStatuses
                .FirstOrDefaultAsync(ps => ps.ProductStatusName != null && ps.ProductStatusName.ToLower() == normalizedName);
        }

        public async Task<int?> FindStatusIdByProductIdAsync(Guid productId)
        {
            // Java: @Query("SELECT p.productStatus.id FROM Product p WHERE p.productId = :productId")
            // This implies directly querying the Product table.
            var product = await _context.Products
                                        .Where(p => p.ProductId == productId)
                                        .Select(p => new { p.ProductStatusId }) // Project only the ID
                                        .FirstOrDefaultAsync();
            return product?.ProductStatusId;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }


        // --- Existing Methods from your provided code (can be removed if not used elsewhere/not in interface) ---
        // Kept for reference or if they were part of an unshown generic interface implementation.
        // If IProductStatusRepository is the sole contract, these are not strictly needed unless you add them back to the interface.

        // Add a new ProductStatus to the database
        public async Task AddAsync(ProductStatus entity, CancellationToken cancellationToken = default)
        {
            await _context.ProductStatuses.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Delete a ProductStatus from the database
        public async Task Delete(ProductStatus entity, CancellationToken cancellationToken = default)
        {
            _context.ProductStatuses.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Find ProductStatuses based on a predicate (filter condition)
        public async Task<IEnumerable<ProductStatus>> FindAsync(Expression<Func<ProductStatus, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.ProductStatuses
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        // Get a ProductStatus based on a specific ProductId - This logic seems to fetch the status itself, not just ID.
        // The interface method FindStatusIdByProductIdAsync is more direct for getting the ID.
        public async Task<ProductStatus?> GetProductStatusByProductIdAsync(Guid productID, CancellationToken cancellationToken = default) // Made return nullable
        {
            return await _context.ProductStatuses
                .FirstOrDefaultAsync(ps => ps.Products.Any(p => p.ProductId == productID), cancellationToken);
            // Original threw exception: ?? throw new ArgumentException("ProductStatus not found for the given productId", nameof(productID));
        }

        // Update an existing ProductStatus
        public async Task Update(ProductStatus entity, CancellationToken cancellationToken = default)
        {
            _context.ProductStatuses.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}