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
    internal class VariationRepository : IVariationRepository
    {
        private readonly UnleashedContext _context;
        public VariationRepository(UnleashedContext context)
        {
            _context = context;
        }

        // Add a new Variation to the database
        public async Task AddAsync(Variation entity, CancellationToken cancellationToken = default)
        {
            await _context.Variations.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Delete a Variation from the database
        public async Task Delete(Variation entity, CancellationToken cancellationToken = default)
        {
            _context.Variations.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Find variations based on a predicate (filter condition)
        public async Task<IEnumerable<Variation>> FindAsync(Expression<Func<Variation, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Variations
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        // Get all variations
        public async Task<IEnumerable<Variation>> GetAllAsync()
        {
            return await _context.Variations.ToListAsync();
        }

        // Get a variation by its ID
        public async Task<Variation> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Variations.AsNoTracking().FirstOrDefaultAsync(x => x.VariationId == id) ??
                   throw new ArgumentException("Variation not found", nameof(id));
        }

        // Get variations based on a ProductId
        public async Task<IEnumerable<Variation>> GetVariationsByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.Variations
                .Where(v => v.ProductId == productId)
                .ToListAsync(cancellationToken);
        }

        // Get variations by color ID
        public async Task<IEnumerable<Variation>> GetVariationsByColorIdAsync(int colorId, CancellationToken cancellationToken = default)
        {
            return await _context.Variations
                .Where(v => v.ColorId == colorId)
                .ToListAsync(cancellationToken);
        }

        // Get variations based on size ID
        public async Task<IEnumerable<Variation>> GetVariationsBySizeIdAsync(int sizeId, CancellationToken cancellationToken = default)
        {
            return await _context.Variations
                .Where(v => v.SizeId == sizeId)
                .ToListAsync(cancellationToken);
        }

        // Get variation by ProductId
        public async Task<Variation> GetVariationByProductIdAsync(Guid productID, CancellationToken cancellationToken = default)
        {
            return await _context.Variations
                .FirstOrDefaultAsync(v => v.ProductId == productID, cancellationToken) ??
                throw new ArgumentException("Variation not found for the given productId", nameof(productID));
        }

        // Update an existing variation
        public async Task Update(Variation entity, CancellationToken cancellationToken = default)
        {
            _context.Variations.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
