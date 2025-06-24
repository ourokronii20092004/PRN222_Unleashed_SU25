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
    internal class SizeRepository : ISizeRepository
    {
        private readonly UnleashedContext _context;

        // Constructor to inject the context
        public SizeRepository(UnleashedContext context)
        {
            _context = context;
        }

        // Add a new Size
        public async Task AddAsync(Size entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            await _context.Sizes.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Delete an existing Size
        public async Task Delete(Size entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            _context.Sizes.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Find Sizes based on a specific condition
        public async Task<IEnumerable<Size>> FindAsync(Expression<Func<Size, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Sizes
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        // Get all Sizes
        public async Task<List<Size>> GetAllAsync()
        {
            return await _context.Sizes.ToListAsync();
        }

        // Get Size by ID
        public async Task<Size?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Sizes
                .FirstOrDefaultAsync(s => s.SizeId == id, cancellationToken);
        }

        // Update an existing Size
        public async Task Update(Size entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            _context.Sizes.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Find Sizes based on ProductId
        public async Task<List<Size>> FindAllByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.Sizes
                .Join(_context.Variations,
                      size => size.SizeId, // Join Size on SizeId
                      variation => variation.SizeId, // Join Variation on SizeId
                      (size, variation) => new { size, variation }) // Project into anonymous type
                .Where(sv => sv.variation.ProductId == productId) // Filter by ProductId from Variation
                .Select(sv => sv.size) // Select only Size entities
                .Distinct() // Ensure unique sizes
                .ToListAsync(cancellationToken);
        }

        // Save changes to the database
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
