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
    public class CartRepository : ICartRepository
    {
        public readonly UnleashedContext _unleashedContext;
        public CartRepository(UnleashedContext unleashedContext)
        {
            _unleashedContext = unleashedContext;
        }
        public async Task AddAsync(Cart entity, CancellationToken cancellationToken = default)
        {
            await _unleashedContext.AddAsync(entity, cancellationToken);
            await _unleashedContext.SaveChangesAsync(cancellationToken);
        }

        public async Task Delete(Cart entity, CancellationToken cancellationToken = default)
        {
            _unleashedContext.Remove(entity);
            await _unleashedContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Cart>> FindAsync(Expression<Func<Cart, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _unleashedContext.Carts.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _unleashedContext.Carts.ToListAsync();
        }

        public async Task<Cart> GetByIdAsync((Guid, int) id, CancellationToken cancellationToken = default)
        {
            var (userId, variationId) = id;
            return await _unleashedContext.Carts.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId && x.VariationId == variationId) ??
                throw new Exception();
        }

        public async Task Update(Cart entity, CancellationToken cancellationToken = default)
        {
            _unleashedContext.Update(entity);
           await _unleashedContext.SaveChangesAsync(cancellationToken);
        }
    }
}
