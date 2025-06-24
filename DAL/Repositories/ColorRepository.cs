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
    public class ColorRepository : IColorRepository
    {
        private readonly UnleashedContext _context;

        public ColorRepository(UnleashedContext context)
        {
            _context = context;
        }

        // Add a new Color
        public async Task AddAsync(Color entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            await _context.Colors.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Delete an existing Color
        public async Task Delete(Color entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            _context.Colors.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Get all Colors by a specific condition
        public async Task<IEnumerable<Color>> FindAsync(Expression<Func<Color, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Colors
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        // Get all Colors
        public async Task<List<Color>> GetAllAsync()
        {
            return await _context.Colors.ToListAsync();
        }

        // Get Color by ID
        public async Task<Color?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Colors
                .FirstOrDefaultAsync(c => c.ColorId == id, cancellationToken);
        }

        // Update an existing Color
        public async Task Update(Color entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            _context.Colors.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // Find Colors based on ProductId
        public async Task<List<Color>> FindAllByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return await _context.Colors
                .Join(_context.Variations,
                      color => color.ColorId, // Join Color on ColorId
                      variation => variation.ColorId, // Join Variation on ColorId
                      (color, variation) => new { color, variation }) // Project into anonymous type
                .Where(cv => cv.variation.ProductId == productId) // Filter by ProductId from Variation
                .Select(cv => cv.color) // Select only Color entities
                .Distinct() // Ensure unique colors
                .ToListAsync(cancellationToken);
        }


        // Save changes to the database
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
