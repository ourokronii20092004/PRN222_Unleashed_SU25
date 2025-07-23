using DAL.Data;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class VariationSingleRepository : IVariationSingleRepository
    {
        private readonly UnleashedContext _context;

        public VariationSingleRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task<VariationSingle> AddAsync(VariationSingle entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _context.VariationSingles.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.VariationSingles.Remove(entity);
            }
        }

        public async Task<List<VariationSingle>> FindByVariationSingleIdsAsync(List<int> variationSingleIds)
        {
            if (variationSingleIds == null || !variationSingleIds.Any())
                return new List<VariationSingle>();

            return await _context.VariationSingles
                .Where(vs => variationSingleIds.Contains(vs.VariationSingleId))
                .ToListAsync();
        }

        public async Task<List<VariationSingle>> GetAllAsync()
        {
            return await _context.VariationSingles.ToListAsync();
        }

        public async Task<VariationSingle?> GetByIdAsync(int id)
        {
            return await _context.VariationSingles.FindAsync(id);
        }

        public async Task UpdateAsync(VariationSingle entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _context.VariationSingles.Update(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
