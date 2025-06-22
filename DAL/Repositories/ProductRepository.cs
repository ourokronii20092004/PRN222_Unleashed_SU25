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
    internal class ProductRepository : IProductRepository
    {
        private readonly UnleashedContext _context;
        public ProductRepository(UnleashedContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Product entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            await _context.Products.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Product entity, CancellationToken cancellationToken = default)
        {
            _context.Products.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> FindAsync(Expression<Func<Product, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _context.Products
               .Where(predicate)
               .ToArrayAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToArrayAsync();
        }

        public async Task<Product> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Products
                .FindAsync(id, cancellationToken);
        }

        public async Task<ProductStatus> GetProductStatusIdAsync(Guid productID, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(productID);

            return await _context.Products
                .Where(p => p.ProductId == productID)  
                .Select(p => p.ProductStatus)          
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task Update(Product entity, CancellationToken cancellationToken = default)
        {
            _context.Products.Update(entity);
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
