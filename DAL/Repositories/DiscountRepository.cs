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
    public class DiscountRepository : IDiscountRepository
    {
        private readonly UnleashedContext _context;

        public DiscountRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task<List<Discount>> GetAllAsync()
        {
            return await _context.Discounts
                .Include(d => d.DiscountStatus)
                .Include(d => d.DiscountType)
                .OrderByDescending(d => d.DiscountCreatedAt)
                .ToListAsync();
        }

        public async Task<Discount?> GetByIdAsync(int id)
        {
            return await _context.Discounts
                .Include(d => d.DiscountStatus)
                .Include(d => d.DiscountType)
                .FirstOrDefaultAsync(d => d.DiscountId == id);
        }

        public async Task<Discount> AddAsync(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
            return discount;
        }

        public Task UpdateAsync(Discount discount)
        {
            _context.Entry(discount).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Discount discount)
        {
            _context.Discounts.Remove(discount);
            return Task.CompletedTask;
        }

        public async Task<bool> CodeExistsAsync(string code, int? excludeId = null)
        {
            var query = _context.Discounts.AsQueryable();
            if (excludeId.HasValue)
            {
                query = query.Where(d => d.DiscountId != excludeId.Value);
            }
            return await query.AnyAsync(d => d.DiscountCode.ToLower() == code.ToLower());
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<List<DiscountStatus>> GetAllStatusesAsync()
        {
            return await _context.DiscountStatuses.ToListAsync();
        }

        public async Task<List<DiscountType>> GetAllTypesAsync()
        {
            return await _context.DiscountTypes.ToListAsync();
        }
    }
}
