using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
        public async Task AddAsync(Color entity, CancellationToken cancellationToken = default)
        {
            await _context.Colors.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Color entity, CancellationToken cancellationToken = default)
        {
            _context.Colors.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Color>> FindAsync(Expression<Func<Color, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Colors
                .Where(predicate)
                .ToArrayAsync(cancellationToken);
        }

        public async Task<IEnumerable<Color>> GetAllAsync()
        {
            return await _context.Colors.ToArrayAsync();
        }

        public async Task<Color> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Colors.AsNoTracking().FirstOrDefaultAsync(x => x.ColorId == id) ??
                throw new ArgumentException();

        }

        public async Task<Color> GetColorByProductIdAsync(Guid productID, CancellationToken cancellationToken = default)
        {
            var color = await _context.Colors
                          .Join(_context.Variations,
                                c => c.ColorId,
                                v => v.ColorId,
                                (c, v) => new { c, v })
                          .Where(joined => joined.v.ProductId == productID)
                          .Select(joined => joined.c)
                          .FirstOrDefaultAsync(cancellationToken);

            return color;
        }

        public async Task Update(Color entity, CancellationToken cancellationToken = default)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
