using DAL.Data;
using DAL.Models;
using DAL.DTOs;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly UnleashedContext _context;

        public BrandRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task<Brand> AddAsync(Brand brand)
        {
            await _context.Brands.AddAsync(brand);
            // SaveChangesAsync will be called by the service or explicitly via this repo's SaveChangesAsync
            return brand;
        }

        public async Task DeleteAsync(Brand brand)
        {
            _context.Brands.Remove(brand);
            // SaveChangesAsync will be called by the service or explicitly
        }

        public async Task<bool> ExistsByNameAsync(string name, int? excludeBrandId = null)
        {
            if (excludeBrandId.HasValue)
            {
                return await _context.Brands.AnyAsync(b => b.BrandName == name && b.BrandId != excludeBrandId.Value);
            }
            return await _context.Brands.AnyAsync(b => b.BrandName == name);
        }

        public async Task<bool> ExistsByWebsiteUrlAsync(string url, int? excludeBrandId = null)
        {
            if (excludeBrandId.HasValue)
            {
                return await _context.Brands.AnyAsync(b => b.BrandWebsiteUrl == url && b.BrandId != excludeBrandId.Value);
            }
            return await _context.Brands.AnyAsync(b => b.BrandWebsiteUrl == url);
        }

        public async Task<List<Brand>> GetAllAsync()
        {
            return await _context.Brands.OrderBy(b => b.BrandId).ToListAsync();
        }

        public async Task<List<BrandDTO>> FindAllBrandsWithQuantityAsync()
        {
            var sql = @"
        SELECT b.brand_id AS BrandId,
               b.brand_name AS BrandName,
               b.brand_description AS BrandDescription,
               b.brand_image_url AS BrandImageUrl,
               b.brand_website_url AS BrandWebsiteUrl,
               b.brand_created_at AS BrandCreatedAt,
               b.brand_updated_at AS BrandUpdatedAt,
               COALESCE(SUM(sv.stock_quantity), 0) AS TotalQuantity
        FROM brand b
        LEFT JOIN product p ON b.brand_id = p.brand_id
        LEFT JOIN variation v ON p.product_id = v.product_id
        LEFT JOIN stock_variation sv ON v.variation_id = sv.variation_id
        GROUP BY b.brand_id, b.brand_name, b.brand_description, b.brand_image_url, b.brand_website_url, b.brand_created_at, b.brand_updated_at
        ORDER BY b.brand_id";

            return await _context.Set<BrandDTO>()
                                 .FromSqlRaw(sql)
                                 .ToListAsync();
        }


        public async Task<Brand?> GetByIdAsync(int id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task UpdateAsync(Brand brand)
        {
            _context.Brands.Update(brand);
            // SaveChangesAsync will be called by the service or explicitly
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}