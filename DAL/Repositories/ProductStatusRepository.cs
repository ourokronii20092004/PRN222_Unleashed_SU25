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
    internal class ProductStatusRepository : IProductStatusRepository
    {
        private readonly UnleashedContext _context;

        public ProductStatusRepository(UnleashedContext context)
        {
            _context = context;
        }

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

        // Get all ProductStatuses
        public async Task<IEnumerable<ProductStatus>> GetAllAsync()
        {
            return await _context.ProductStatuses.ToListAsync();
        }

        // Get a ProductStatus by its ID
        public async Task<ProductStatus> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.ProductStatuses.AsNoTracking().FirstOrDefaultAsync(x => x.ProductStatusId == id) ??
                throw new ArgumentException("ProductStatus not found", nameof(id));
        }

        // Get a ProductStatus based on a specific ProductId
        public async Task<ProductStatus> GetProductStatusByProductIdAsync(Guid productID, CancellationToken cancellationToken = default)
        {
            return await _context.ProductStatuses
                .Where(ps => ps.Products.Any(p => p.ProductId == productID)) // Check if any product has the given productID
                .FirstOrDefaultAsync(cancellationToken) ??
                throw new ArgumentException("ProductStatus not found for the given productId", nameof(productID));
        }

        // Update an existing ProductStatus
        public async Task Update(ProductStatus entity, CancellationToken cancellationToken = default)
        {
            _context.ProductStatuses.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
