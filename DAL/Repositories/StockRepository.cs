using DAL.Data;
using DAL.DTOs.StockDTOs;
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
    public class StockRepository : IStockRepository
    {
        private readonly UnleashedContext _context;

        public StockRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task<Stock> AddAsync(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            // SaveChangesAsync typically called by service or unit of work
            return stock;
        }

        public async Task DeleteAsync(int id)
        {
            var stock = await _context.Stocks.FindAsync(id);
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
            }
            // SaveChangesAsync typically called by service
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Stocks.AnyAsync(s => s.StockId == id);
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.OrderBy(s => s.StockName).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.FindAsync(id);
        }

        public async Task<List<StockDetailDTO>> GetStockDetailsByIdAsync(int stockId)
        {
            // This is the translation of your native query from Java.
            // Ensure column aliases in SQL (AS ColumnName) match StockDetailDTO property names
            // or that StockDetailDTO is configured as a keyless entity type in OnModelCreating
            // for EF Core to map them correctly.
            var sql = $@"
                SELECT DISTINCT
                    s.stock_id AS StockId,
                    s.stock_name AS StockName,
                    s.stock_address AS StockAddress,
                    v.variation_id AS VariationId,
                    v.variation_price AS ProductPrice,
                    v.variation_image AS ProductVariationImage,
                    p.product_name AS ProductName,
                    p.product_id AS ProductId,
                    b.brand_id AS BrandId,
                    b.brand_name AS BrandName,
                    ct.category_id AS CategoryId,
                    ct.category_name AS CategoryName,
                    sz.size_name AS SizeName,
                    cl.color_name AS ColorName,
                    cl.color_hex_code AS HexCode,
                    sv.stock_quantity AS Quantity
                FROM
                    stock s
                LEFT JOIN stock_variation sv ON s.stock_id = sv.stock_id
                LEFT JOIN variation v ON sv.variation_id = v.variation_id
                LEFT JOIN product p ON v.product_id = p.product_id
                LEFT JOIN brand b ON p.brand_id = b.brand_id
                LEFT JOIN product_category pc ON p.product_id = pc.product_id -- Assuming product_category joins Product and Category
                LEFT JOIN category ct ON pc.category_id = ct.category_id
                LEFT JOIN size sz ON v.size_id = sz.size_id
                LEFT JOIN color cl ON v.color_id = cl.color_id
                WHERE s.stock_id = {{0}}"; // Use {{0}} for parameter placeholder in FromSqlRaw

            // If StockDetailDTO is configured as keyless entity type in OnModelCreating:
            return await _context.Set<StockDetailDTO>()
                                 .FromSqlRaw(sql, stockId)
                                 .ToListAsync();

            // Alternative if not configured as keyless (less common for complex types like this):
            // return await _context.Database
            //                      .SqlQueryRaw<StockDetailDTO>(sql, stockId)
            //                      .ToListAsync();
        }

        public async Task UpdateAsync(Stock stock)
        {
            _context.Entry(stock).State = EntityState.Modified;
            // SaveChangesAsync typically called by service
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}