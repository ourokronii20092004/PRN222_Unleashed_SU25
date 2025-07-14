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
    public class OrderRepository : IOrderRepository
    {
        public UnleashedContext _unleashedcontext;

        public OrderRepository(UnleashedContext unleashedcontext)
        {
            _unleashedcontext = unleashedcontext;
        }

        public async Task AddAsync(Order order)
        {
            await _unleashedcontext.Orders.AddAsync(order);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _unleashedcontext.Orders.Include(o => o.OrderStatus)
                .Include(o => o.User)
                .Include(o => o.OrderVariationSingles).ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(Guid id)
        {
            return await _unleashedcontext.Orders.Include(o => o.OrderStatus)
                .Include(o => o.User)
                .Include(o => o.OrderVariationSingles)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid userId)
        {
            return await _unleashedcontext.Orders.Where(o => o.UserId == userId)
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderVariationSingles).ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _unleashedcontext.SaveChangesAsync();
        }

        public void Update(Order order)
        {
            _unleashedcontext.Orders.Update(order);

        }
    }
}
