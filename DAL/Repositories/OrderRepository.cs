using AutoMapper;
using DAL.Data;
using DAL.DTOs.OderDTOs;
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
        private readonly IMapper _mapper;
        public OrderRepository(UnleashedContext unleashedcontext, IMapper mapper)
        {
            _unleashedcontext = unleashedcontext;
            _mapper = mapper;
        }

        public async Task AddAsync(Order order)
        {
            await _unleashedcontext.Orders.AddAsync(order);
        }

        public async Task AddAsync(OrderDTO order)
        {
            // Map OrderDTO to Order entity
            var order1 = _mapper.Map<Order>(order);

            // Set default values if needed
            order.OrderId = Guid.NewGuid();
            order.OrderDate = DateTimeOffset.Now;
            order.OrderStatusId = 5; // Pending status

            await _unleashedcontext.Orders.AddAsync(order1);

            // Add order details if they exist in the DTO
            if (order.OrderItems != null && order.OrderItems.Any())
            {
                foreach (var itemDto in order.OrderItems)
                {
                    var orderDetail = _mapper.Map<OrderDetailDTO>(itemDto);
                    orderDetail.OrderId = order.OrderId;
                    await _unleashedcontext.AddAsync(orderDetail);
                }
            }

            await _unleashedcontext.SaveChangesAsync();
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
