using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BLL.Service
{
    public class BrandService : IBrandService
    {
        private readonly UnleashedContext _context;

        public BrandService(UnleashedContext context)
        {
            _context = context;
        }

        public async Task<List<Brand>> GetAllBrandsAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await _context.Brands.FindAsync(id);
        }

        public async Task CreateBrandAsync(Brand brand)
        {
            brand.BrandCreatedAt = DateTime.UtcNow;
            brand.BrandUpdatedAt = DateTime.UtcNow;
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateBrandAsync(Brand brand)
        {
            brand.BrandUpdatedAt = DateTime.UtcNow;
            _context.Update(brand);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BrandExistsAsync(brand.BrandId))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeleteBrandAsync(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return false;
            }

            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BrandExistsAsync(int id)
        {
            return await _context.Brands.AnyAsync(e => e.BrandId == id);
        }
    }
}
