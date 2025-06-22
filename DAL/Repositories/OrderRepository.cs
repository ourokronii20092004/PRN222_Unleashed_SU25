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
        public async Task AddAsync(Order entity, CancellationToken cancellationToken = default)
        {
            await _unleashedcontext.AddAsync(entity, cancellationToken);
            await _unleashedcontext.SaveChangesAsync();
        }

        public async Task Delete(Order entity, CancellationToken cancellationToken = default)
        {
            _unleashedcontext.Orders.Remove(entity);
            await _unleashedcontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> FindAsync(Expression<Func<Order, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _unleashedcontext.Orders.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _unleashedcontext.Orders.ToListAsync();
        }

        public async Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _unleashedcontext.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == id)?? 
                throw new ArgumentException();
        }

        public async Task<IEnumerable<Order>> GetOrderListByUserId(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _unleashedcontext.Orders.Where(o => o.UserId == userId).ToListAsync(cancellationToken);
        }

        public async Task Update(Order entity, CancellationToken cancellationToken = default)
        {
            _unleashedcontext.Update(entity);
            await _unleashedcontext.SaveChangesAsync();
        }
    }
}
