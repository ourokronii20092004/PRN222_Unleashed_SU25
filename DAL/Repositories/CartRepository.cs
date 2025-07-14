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

        public async Task AddOrUpdateAsync(Cart cart)
        {
            var existing = await _unleashedContext.Carts.FirstOrDefaultAsync(c => c.UserId == cart.UserId && c.VariationId == cart.VariationId);
            if (existing == null)
            {
                await _unleashedContext.Carts.AddAsync(cart);
            }
            else
            {
                existing.CartQuantity = (existing.CartQuantity ?? 0) + (cart.CartQuantity ?? 0);
                _unleashedContext.Carts.Update(existing);
            }
            await _unleashedContext.SaveChangesAsync();
        }

        public async Task<List<Cart>> GetAllByUserIdAsync(Guid userId)
        {
            return await _unleashedContext.Carts.Include(c => c.Variation).ThenInclude(v => v.Product).Where(c => c.UserId == userId).ToListAsync();
        }

        public async Task RemoveAllAsync(Guid userId)
        {
            var carts = _unleashedContext.Carts.Where(c => c.UserId == userId);
            if (!carts.Any())
            {
                throw new Exception("no item in cart");
                _unleashedContext.Carts.RemoveRange(carts);
                await _unleashedContext.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(Guid userId, int variationId)
        {
            
            var cart = await _unleashedContext.Carts.FirstOrDefaultAsync(c => c.UserId == userId && c.VariationId == variationId);
            if (cart == null)
            {
                _unleashedContext.Carts.Remove(cart);
                await _unleashedContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Cart not found");
            }
        }
    }
}
