using DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public class ProductRepository : IProductRepository
    {
        private readonly UnleashedContext _context;

        public ProductRepository(UnleashedContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByBrandAsync(int brandId)
        {
            return await _context.Products.AnyAsync(p => p.BrandId == brandId); // Assuming Product has BrandId FK
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
