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
    public class StockVariationRepository : IStockVariationRepository
    {
        private readonly UnleashedContext _context;

        public StockVariationRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task AddAsync(StockVariation stockVariation)
        {
            await _context.StockVariations.AddAsync(stockVariation);
        }

        public async Task DeleteAsync(StockVariation stockVariation)
        {
            _context.StockVariations.Remove(stockVariation);
        }

        public async Task<List<StockVariation>> FindByVariationIdAsync(int variationId)
        {
            return await _context.StockVariations
                .Where(sv => sv.VariationId == variationId)
                .ToListAsync();
        }

        public async Task<List<StockVariation>> GetAllAsync()
        {
            return await _context.StockVariations.ToListAsync();
        }

        public async Task<StockVariation?> GetByIdAsync(int variationId, int stockId)
        {
            // For composite keys, FindAsync takes key values in order they are defined in HasKey()
            return await _context.StockVariations.FindAsync(variationId, stockId);
        }

        public async Task<int?> GetTotalStockQuantityForProductAsync(Guid productId)
        {
            // Translates:
            // @Query("SELECT SUM(sv.stockQuantity) FROM StockVariation sv " +
            //        "JOIN Variation v ON sv.id.variationId = v.id " +
            //        "JOIN Product p ON v.product.productId = p.productId " +
            //        "WHERE p.productId = :productId")
            return await _context.StockVariations
                .Where(sv => sv.Variation.ProductId == productId) // Relies on navigation property
                .SumAsync(sv => sv.StockQuantity);

            // If navigation property isn't efficient or available for some reason, an alternative join:
            // return await (from sv in _context.StockVariations
            //               join v in _context.Variations on sv.VariationId equals v.VariationId
            //               where v.ProductId == productId
            //               select sv.StockQuantity)
            //              .SumAsync();
        }

        public async Task<int?> GetTotalStockQuantityForVariationAsync(int variationId)
        {
            // Translates:
            // @Query("SELECT SUM(sv.stockQuantity) FROM StockVariation sv WHERE sv.id.variationId = :variationId")
            return await _context.StockVariations
                .Where(sv => sv.VariationId == variationId)
                .SumAsync(sv => sv.StockQuantity);
        }

        public async Task UpdateAsync(StockVariation stockVariation)
        {
            _context.Entry(stockVariation).State = EntityState.Modified;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}